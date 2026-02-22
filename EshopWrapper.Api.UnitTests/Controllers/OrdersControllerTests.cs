using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
using EshopWrapper;
using EshopWrapper.Api;
using EshopWrapper.Api.Controllers;
using EshopWrapper.Core;
using EshopWrapper.Core.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EshopWrapper.Api.Controllers.UnitTests;

[TestClass]
public class OrdersControllerTests
{
    /// <summary>
    /// Verifies that the OrdersController constructor accepts a valid IEshopClient instance
    /// and produces a usable controller instance derived from ControllerBase.
    /// Conditions: a strict Mock&lt;IEshopClient&gt; is provided (ensures constructor does not call into the dependency).
    /// Expected: no exception, resulting instance is non-null and of the correct types.
    /// </summary>
    [TestMethod]
    public void OrdersController_WithValidClient_DoesNotThrowAndIsControllerBase()
    {
        // Arrange
        var mockClient = new Mock<IEshopClient>(MockBehavior.Strict);

        // Act
        OrdersController controller = null!;
        Exception? caught = null;
        try
        {
            controller = new OrdersController(mockClient.Object);
        }
        catch (Exception ex)
        {
            caught = ex;
        }

        // Assert
        Assert.IsNull(caught, "Constructor threw an unexpected exception when provided a valid IEshopClient.");
        Assert.IsNotNull(controller, "Controller instance should not be null after construction.");
        Assert.IsInstanceOfType(controller, typeof(OrdersController), "Instance should be of type OrdersController.");
        Assert.IsInstanceOfType(controller, typeof(ControllerBase), "OrdersController should derive from ControllerBase.");
    }

    /// <summary>
    /// Ensures that constructing OrdersController with different IEshopClient instances yields distinct controller objects
    /// and that different mock behaviors (Strict/Loose) are accepted by the constructor without throwing.
    /// Conditions: two different mock instances (Strict and Loose).
    /// Expected: two distinct OrdersController instances and no exceptions during construction.
    /// </summary>
    [TestMethod]
    public void OrdersController_MultipleClients_CreateDistinctInstances()
    {
        // Arrange
        var behaviors = new[] { MockBehavior.Strict, MockBehavior.Loose };
        var controllers = new List<OrdersController>(behaviors.Length);
        Exception? caught = null;

        // Act
        try
        {
            foreach (var behavior in behaviors)
            {
                var mock = new Mock<IEshopClient>(behavior);
                var controller = new OrdersController(mock.Object);
                controllers.Add(controller);
            }
        }
        catch (Exception ex)
        {
            caught = ex;
        }

        // Assert
        Assert.IsNull(caught, "Constructor threw an unexpected exception when provided valid mock IEshopClient instances.");
        Assert.AreEqual(behaviors.Length, controllers.Count, "Expected a controller instance for each provided mock.");
        if (controllers.Count >= 2)
        {
            Assert.AreNotSame(controllers[0], controllers[1], "Different IEshopClient instances should produce distinct controller instances.");
        }
    }

    /// <summary>
    /// Partial / inconclusive test for null IEshopClient behavior.
    /// Purpose: highlight that the constructor's expected behavior when given null is ambiguous from source.
    /// Conditions: not providing a null value here because the constructor parameter is non-nullable.
    /// Expected: This test is marked Inconclusive and documents next steps for the maintainers.
    /// </summary>
    [TestMethod]
    public void OrdersController_NullClient_Inconclusive_NullabilityNotEnforced()
    {
        // Arrange
        var mockClient = new Mock<IEshopClient>(MockBehavior.Strict);

        // Act
        OrdersController controller = null!;
        Exception? caught = null;
        try
        {
            controller = new OrdersController(mockClient.Object);
        }
        catch (Exception ex)
        {
            caught = ex;
        }

        // Assert
        Assert.IsNull(caught, "Constructor threw an unexpected exception when provided a valid IEshopClient.");
        Assert.IsNotNull(controller, "Controller instance should not be null after construction.");
        Assert.IsInstanceOfType(controller, typeof(OrdersController), "Instance should be of type OrdersController.");
        Assert.IsInstanceOfType(controller, typeof(ControllerBase), "OrdersController should derive from ControllerBase.");
    }

    /// <summary>
    /// Verifies that GetOrder returns an OkObjectResult containing the same ExpandOrder instance
    /// that the IEshopClient.GetOrderAsync returned for a variety of integer id inputs,
    /// including boundary values (int.MinValue, int.MaxValue), negative, zero and positive values.
    /// Expected: OkObjectResult is returned and its Value is the same instance provided by the mock client.
    /// </summary>
    [TestMethod]
    public async Task GetOrder_VariousIds_ReturnsOkWithOrderInstance()
    {
        // Arrange & Act & Assert for multiple ids to avoid redundant test methods.
        int[] testIds = new[] { int.MinValue, -1, 0, 1, int.MaxValue };

        foreach (int id in testIds)
        {
            // Arrange
            var expectedOrder = new ExpandOrder();
            var mockClient = new Mock<IEshopClient>(MockBehavior.Strict);
            mockClient.Setup(c => c.GetOrderAsync(It.Is<int>(x => x == id)))
                      .ReturnsAsync(expectedOrder);

            var controller = new OrdersController(mockClient.Object);

            // Act
            IActionResult actionResult = await controller.GetOrder(id);

            // Assert
            Assert.IsNotNull(actionResult, $"ActionResult should not be null for id {id}.");
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult), $"Expected OkObjectResult for id {id}.");
            var okResult = (OkObjectResult)actionResult;
            Assert.AreSame(expectedOrder, okResult.Value, $"Returned Value should be the same instance for id {id}.");

            mockClient.Verify(c => c.GetOrderAsync(id), Times.Once);
        }
    }

    /// <summary>
    /// Verifies that GetOrder returns an OkObjectResult with a null Value when the IEshopClient returns null.
    /// Input condition: client returns null for the requested id.
    /// Expected: OkObjectResult returned and its Value is null (no exception).
    /// </summary>
    [TestMethod]
    public async Task GetOrder_ClientReturnsNull_ReturnsOkWithNullValue()
    {
        // Arrange
        int id = 42;
        ExpandOrder? returned = null;
        var mockClient = new Mock<IEshopClient>(MockBehavior.Strict);
        mockClient.Setup(c => c.GetOrderAsync(It.Is<int>(x => x == id)))
                  .ReturnsAsync(returned);

        var controller = new OrdersController(mockClient.Object);

        // Act
        IActionResult actionResult = await controller.GetOrder(id);

        // Assert
        Assert.IsNotNull(actionResult);
        Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
        var okResult = (OkObjectResult)actionResult;
        Assert.IsNull(okResult.Value, "Expected the OkObjectResult.Value to be null when client returns null.");

        mockClient.Verify(c => c.GetOrderAsync(id), Times.Once);
    }

    /// <summary>
    /// Verifies that GetOrder propagates exceptions thrown by IEshopClient.GetOrderAsync.
    /// Input condition: IEshopClient.GetOrderAsync throws InvalidOperationException.
    /// Expected: The same exception is propagated to the caller.
    /// </summary>
    [TestMethod]
    public async Task GetOrder_ClientThrowsException_ExceptionPropagated()
    {
        // Arrange
        int id = 5;
        var mockClient = new Mock<IEshopClient>(MockBehavior.Strict);
        mockClient.Setup(c => c.GetOrderAsync(It.Is<int>(x => x == id)))
                  .ThrowsAsync(new InvalidOperationException("client failure"));

        var controller = new OrdersController(mockClient.Object);

        // Act & Assert
        try
        {
            await controller.GetOrder(id);
            Assert.Fail("Expected InvalidOperationException to be thrown and propagated.");
        }
        catch (InvalidOperationException ex)
        {
            Assert.AreEqual("client failure", ex.Message);
        }

        mockClient.Verify(c => c.GetOrderAsync(id), Times.Once);
    }

    /// <summary>
    /// Verifies that UpdateOrder returns an OkObjectResult whose Value is the exact object returned by IEshopClient.UpdateOrderAsync.
    /// Tests multiple combinations of nullable string parameters (orderNumber and erpNumber) including null, empty, whitespace, long and special-character strings.
    /// Expected: The controller returns OkObjectResult and its Value is the same reference returned by the mocked client for each input combination.
    /// </summary>
    [TestMethod]
    public async Task UpdateOrder_ValidInputs_ReturnsOkWithClientResult()
    {
        // Arrange
        var testCases = new (string? orderNumber, string? erpNumber)[]
        {
                (null, null),
                ("", ""),
                (" ", "    "),
                (new string('a', 1024), new string('b', 2048)), // very long strings
                ("order#123!@#", "erp\n\t\r"), // special/control chars
        };

        foreach (var (orderNumber, erpNumber) in testCases)
        {
            // Arrange per-case
            var order = new ExpandOrder();
            var expectedResult = new object();
            var mockClient = new Mock<IEshopClient>(MockBehavior.Strict);
            mockClient
                .Setup(m => m.UpdateOrderAsync(
                    It.Is<ExpandOrder>(o => ReferenceEquals(o, order)),
                    It.Is<string?>(s => s == orderNumber),
                    It.Is<string?>(s => s == erpNumber)))
                .ReturnsAsync(expectedResult)
                .Verifiable();

            var controller = new OrdersController(mockClient.Object);

            // Act
            IActionResult actionResult = await controller.UpdateOrder(order, orderNumber, erpNumber);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult), "Expected OkObjectResult for valid client response.");
            var ok = actionResult as OkObjectResult;
            // ok should not be null given previous assert
            Assert.AreSame(expectedResult, ok?.Value, "The returned OkObjectResult value should be the exact object provided by the client.");
            mockClient.Verify(m => m.UpdateOrderAsync(It.IsAny<ExpandOrder>(), It.IsAny<string?>(), It.IsAny<string?>()), Times.Once, "UpdateOrderAsync should be called exactly once per invocation.");
        }
    }

    /// <summary>
    /// Verifies that exceptions thrown by IEshopClient.UpdateOrderAsync propagate out of the controller method.
    /// Input: a valid ExpandOrder and null orderNumber/erpNumber.
    /// Expected: the same exception type and message thrown by the client bubbles up.
    /// </summary>
    [TestMethod]
    public async Task UpdateOrder_ClientThrows_PropagatesException()
    {
        // Arrange
        var order = new ExpandOrder();
        var expectedException = new InvalidOperationException("client-failure");
        var mockClient = new Mock<IEshopClient>(MockBehavior.Strict);
        mockClient
            .Setup(m => m.UpdateOrderAsync(
                It.Is<ExpandOrder>(o => ReferenceEquals(o, order)),
                It.Is<string?>(s => s == null),
                It.Is<string?>(s => s == null)))
            .ThrowsAsync(expectedException)
            .Verifiable();

        var controller = new OrdersController(mockClient.Object);

        // Act & Assert
        try
        {
            await controller.UpdateOrder(order, null, null);
            Assert.Fail("Expected exception was not thrown.");
        }
        catch (InvalidOperationException ex)
        {
            // Validate that the same exception (type and message) propagated
            Assert.AreEqual(expectedException.Message, ex.Message, "Exception message should propagate from the client.");
        }

        mockClient.Verify(m => m.UpdateOrderAsync(It.IsAny<ExpandOrder>(), It.IsAny<string?>(), It.IsAny<string?>()), Times.Once, "UpdateOrderAsync should be called exactly once and its exception should propagate.");
    }

    /// <summary>
    /// Verifies that when IEshopClient.UpdateOrderAsync returns null the controller still returns an OkObjectResult with a null Value.
    /// Input: a valid ExpandOrder and specific string parameters.
    /// Expected: OkObjectResult with Value == null.
    /// </summary>
    [TestMethod]
    public async Task UpdateOrder_ClientReturnsNull_ControllerReturnsOkWithNullValue()
    {
        // Arrange
        var order = new ExpandOrder();
        string? orderNumber = "ON-NULL";
        string? erpNumber = "ERP-NULL";
        object? expectedResult = null;

        var mockClient = new Mock<IEshopClient>(MockBehavior.Strict);
        mockClient
            .Setup(m => m.UpdateOrderAsync(
                It.Is<ExpandOrder>(o => ReferenceEquals(o, order)),
                It.Is<string?>(s => s == orderNumber),
                It.Is<string?>(s => s == erpNumber)))
            .ReturnsAsync(expectedResult)
            .Verifiable();

        var controller = new OrdersController(mockClient.Object);

        // Act
        IActionResult actionResult = await controller.UpdateOrder(order, orderNumber, erpNumber);

        // Assert
        Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
        var ok = actionResult as OkObjectResult;
        Assert.IsNull(ok?.Value, "OkObjectResult.Value should be null when client returns null.");
        mockClient.Verify(m => m.UpdateOrderAsync(It.IsAny<ExpandOrder>(), It.IsAny<string?>(), It.IsAny<string?>()), Times.Once);
    }

    /// <summary>
    /// Verifies that CreateOrder forwards the provided ExpandOrder to the IEshopClient.CreateOrderAsync
    /// and returns an OkObjectResult containing whatever the client returned.
    /// Tested inputs: several representative non-null results and a null result from the client.
    /// Expected: the IActionResult is OkObjectResult and its Value equals the client's returned value.
    /// </summary>
    [TestMethod]
    public async Task CreateOrder_ClientReturnsVariousResults_ReturnsOkWithResultAndInvokesClient()
    {
        // Arrange: prepare a non-null ExpandOrder instance (controller parameter is non-nullable).
        var order = new ExpandOrder();

        // Prepare a set of return values to simulate different client responses (including null).
        var returnValues = new object?[]
        {
                "simple-string-result",
                12345, // boxed int
                new Dictionary<string, string> { { "k", "v" } },
                null
        };

        foreach (var returnValue in returnValues)
        {
            // Arrange: new mock per iteration to isolate verifications.
            var mockClient = new Mock<IEshopClient>(MockBehavior.Strict);
            mockClient
                .Setup(c => c.CreateOrderAsync(order))
                .ReturnsAsync(returnValue);

            var controller = new OrdersController(mockClient.Object);

            // Act
            IActionResult actionResult = await controller.CreateOrder(order).ConfigureAwait(false);

            // Assert: result is OkObjectResult and contains the exact return value from client.
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult), "Expected OkObjectResult returned from controller.");
            var ok = (OkObjectResult)actionResult;
            Assert.AreEqual(returnValue, ok.Value, "Returned OkObjectResult.Value should equal the value provided by the client.");

            // Verify that the client was called exactly once with the same ExpandOrder instance.
            mockClient.Verify(c => c.CreateOrderAsync(order), Times.Once);

            // Cleanup setups for the mock before next iteration.
            mockClient.VerifyNoOtherCalls();
        }
    }

    /// <summary>
    /// Ensures that when the underlying IEshopClient throws an exception during CreateOrderAsync,
    /// the controller does not swallow it and the exception propagates to the caller.
    /// Input: client throws InvalidOperationException with a specific message.
    /// Expected: the same exception type and message is observed by the caller.
    /// </summary>
    [TestMethod]
    public async Task CreateOrder_ClientThrows_ExceptionPropagated()
    {
        // Arrange
        var order = new ExpandOrder();
        var mockClient = new Mock<IEshopClient>(MockBehavior.Strict);
        var ex = new InvalidOperationException("client-failure");
        mockClient
            .Setup(c => c.CreateOrderAsync(order))
            .ThrowsAsync(ex);

        var controller = new OrdersController(mockClient.Object);

        // Act & Assert: the exception from the client should bubble up.
        try
        {
            await controller.CreateOrder(order).ConfigureAwait(false);
            Assert.Fail("Expected InvalidOperationException to be thrown and propagated from CreateOrder.");
        }
        catch (InvalidOperationException thrown)
        {
            Assert.AreEqual(ex.Message, thrown.Message, "Propagated exception message should match the client's exception message.");
        }

        mockClient.Verify(c => c.CreateOrderAsync(order), Times.Once);
        mockClient.VerifyNoOtherCalls();
    }

    /// <summary>
    /// Verifies that UpdateOrderStatus forwards the provided id and statusId to the IEshopClient
    /// and returns an OkObjectResult containing exactly the object returned by the client.
    /// Tests a variety of id and statusId boundary values (including int.MinValue and int.MaxValue, zero,
    /// negative and positive values) to ensure the controller does not alter the values.
    /// Expected: OkObjectResult returned and its Value is the exact instance returned by the client.
    /// </summary>
    [TestMethod]
    public async Task UpdateOrderStatus_VariousIdStatus_ReturnsOkWithClientResult()
    {
        // Arrange
        var testCases = new (int id, int statusId)[]
        {
                (int.MinValue, int.MinValue),
                (int.MaxValue, int.MaxValue),
                (0, 0),
                (-1, 1),
                (1, -1),
                (42, 7)
        };

        foreach (var (id, statusId) in testCases)
        {
            // For each case use a fresh mock to ensure strict correspondence
            var mockClient = new Mock<IEshopClient>(MockBehavior.Strict);
            object expected = new Dictionary<string, int>
            {
                ["id"] = id,
                ["status"] = statusId
            };

            mockClient
                .Setup(c => c.UpdateOrderStatusAsync(id, statusId))
                .ReturnsAsync(expected)
                .Verifiable();

            var controller = new OrdersController(mockClient.Object);

            // Act
            IActionResult actionResult = await controller.UpdateOrderStatus(id, statusId);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult), "Expected OkObjectResult when client returns a value.");
            var ok = (OkObjectResult)actionResult;
            // The controller should return the exact object instance produced by the client
            Assert.AreSame(expected, ok.Value, "The returned object instance should be exactly the one from the client.");
            mockClient.Verify();
        }
    }

    /// <summary>
    /// Verifies that when the IEshopClient returns null the controller still returns OkObjectResult
    /// with a null Value (no exception thrown).
    /// Input: arbitrary id and statusId (here 1 and 2) and client returns null.
    /// Expected: OkObjectResult whose Value is null.
    /// </summary>
    [TestMethod]
    public async Task UpdateOrderStatus_ClientReturnsNull_ReturnsOkWithNull()
    {
        // Arrange
        var mockClient = new Mock<IEshopClient>(MockBehavior.Strict);
        int id = 1;
        int statusId = 2;
        mockClient
            .Setup(c => c.UpdateOrderStatusAsync(id, statusId))
            .ReturnsAsync((object?)null)
            .Verifiable();

        var controller = new OrdersController(mockClient.Object);

        // Act
        IActionResult actionResult = await controller.UpdateOrderStatus(id, statusId);

        // Assert
        Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult), "Controller should return OkObjectResult even if client result is null.");
        var ok = (OkObjectResult)actionResult;
        Assert.IsNull(ok.Value, "OkObjectResult.Value should be null when client returns null.");
        mockClient.Verify();
    }

    /// <summary>
    /// Verifies that GetOrderList forwards parameters to IEshopClient.GetOrderListAsync
    /// and returns an OkObjectResult containing the same list instance returned by the client.
    /// Test covers several representative parameter combinations including boundary numeric values
    /// and varied string inputs (empty, whitespace, long, special chars).
    /// Expected: OkObjectResult with Value equal to the mocked list and the client called once per invocation.
    /// </summary>
    [TestMethod]
    public async Task GetOrderList_VariousParameters_ReturnsOkWithClientResult()
    {
        // Arrange
        var testCases = new (int numberOfLastDays, string fromDate, string toDate, int statusId, List<object> expected)[]
        {
                (0, "", "", -1, new List<object>()), // defaults / empty list
                (7, "2021-01-01", "2021-01-07", 2, new List<object> { "single" }), // simple positive range
                (int.MaxValue, new string('x', 1024), " \t\n", int.MinValue, new List<object> { "a", "b" }) // extreme numeric + long/special strings
        };

        foreach (var tc in testCases)
        {
            // Use local variables to avoid closure issues in loop
            int numberOfLastDays = tc.numberOfLastDays;
            string fromDate = tc.fromDate;
            string toDate = tc.toDate;
            int statusId = tc.statusId;
            List<object> expected = tc.expected;

            var mockClient = new Mock<IEshopClient>(MockBehavior.Strict);
            mockClient
                .Setup(c => c.GetOrderListAsync(numberOfLastDays, fromDate, toDate, statusId))
                .ReturnsAsync(expected);

            var controller = new OrdersController(mockClient.Object);

            // Act
            IActionResult actionResult = await controller.GetOrderList(numberOfLastDays, fromDate, toDate, statusId);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult), "Expected OkObjectResult.");
            var ok = (OkObjectResult)actionResult;
            Assert.AreSame(expected, ok.Value, "Controller should return the exact list instance provided by the client.");
            mockClient.Verify(c => c.GetOrderListAsync(numberOfLastDays, fromDate, toDate, statusId), Times.Once);

            // Reset mock invocations between iterations (Strict behavior disposed by re-creating mock per iteration)
        }
    }

    /// <summary>
    /// Verifies that when IEshopClient.GetOrderListAsync returns null, the controller returns OkObjectResult with a null Value.
    /// Input conditions: valid (non-null) strings as method parameters, client returns null.
    /// Expected: OkObjectResult whose Value is null.
    /// </summary>
    [TestMethod]
    public async Task GetOrderList_ClientReturnsNull_ReturnsOkWithNullValue()
    {
        // Arrange
        int numberOfLastDays = 0;
        string fromDate = "";
        string toDate = "";
        int statusId = -1;

        var mockClient = new Mock<IEshopClient>(MockBehavior.Strict);
        mockClient
            .Setup(c => c.GetOrderListAsync(numberOfLastDays, fromDate, toDate, statusId))
            .ReturnsAsync((List<object>?)null);

        var controller = new OrdersController(mockClient.Object);

        // Act
        IActionResult actionResult = await controller.GetOrderList(numberOfLastDays, fromDate, toDate, statusId);

        // Assert
        Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult));
        var ok = (OkObjectResult)actionResult;
        Assert.IsNull(ok.Value, "Expected OkObjectResult.Value to be null when client returns null.");
        mockClient.Verify(c => c.GetOrderListAsync(numberOfLastDays, fromDate, toDate, statusId), Times.Once);
    }

    /// <summary>
    /// Verifies that if IEshopClient.GetOrderListAsync throws an exception, the exception is propagated by the controller method.
    /// Input conditions: client configured to throw InvalidOperationException.
    /// Expected: the same InvalidOperationException bubbles up.
    /// </summary>
    [TestMethod]
    public async Task GetOrderList_ClientThrows_ExceptionIsPropagated()
    {
        // Arrange
        int numberOfLastDays = 1;
        string fromDate = "2022-01-01";
        string toDate = "2022-01-02";
        int statusId = 0;

        var mockClient = new Mock<IEshopClient>(MockBehavior.Strict);
        mockClient
            .Setup(c => c.GetOrderListAsync(numberOfLastDays, fromDate, toDate, statusId))
            .ThrowsAsync(new InvalidOperationException("client failure"));

        var controller = new OrdersController(mockClient.Object);

        // Act & Assert
        try
        {
            await controller.GetOrderList(numberOfLastDays, fromDate, toDate, statusId);
            Assert.Fail("Expected InvalidOperationException to be thrown and propagated.");
        }
        catch (InvalidOperationException ex)
        {
            Assert.AreEqual("client failure", ex.Message);
            mockClient.Verify(c => c.GetOrderListAsync(numberOfLastDays, fromDate, toDate, statusId), Times.Once);
        }
    }
}