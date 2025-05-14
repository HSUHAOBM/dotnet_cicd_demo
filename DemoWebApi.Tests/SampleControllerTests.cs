using DemoWebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Xunit;

namespace DemoWebApi.Tests
{
    public class SampleControllerTests
    {
        // 測試是否正確回傳預設的三筆資料
        [Fact]
        public void GetAll_ReturnsSeededItems()
        {
            var controller = new SampleController();

            var result = controller.GetAll() as OkObjectResult;

            Assert.NotNull(result);
            var items = Assert.IsType<List<string>>(result.Value);
            Assert.Equal(new List<string> { "Apple", "Banana", "Carrot" }, items);
        }

        // 測試以正確的索引取得指定項目（預設第 1 筆應為 "Banana"）
        [Fact]
        public void GetById_ReturnsBanana()
        {
            var controller = new SampleController();

            var result = controller.GetById(1) as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal("Banana", result.Value);
        }

        // 測試使用超過最大索引的情況，應回傳 NotFound
        [Fact]
        public void GetById_InvalidId_ReturnsNotFound()
        {
            var controller = new SampleController();

            var result = controller.GetById(99);

            Assert.IsType<NotFoundResult>(result);
        }

        // 測試使用負數索引的情況，應回傳 NotFound
        [Fact]
        public void GetById_NegativeId_ReturnsNotFound()
        {
            var controller = new SampleController();

            var result = controller.GetById(-1);

            Assert.IsType<NotFoundResult>(result);
        }

        // 測試新增合法項目（"Durian"），應成功回傳 CreatedAtAction 結果
        [Fact]
        public void Add_ValidItem_ReturnsCreated()
        {
            var controller = new SampleController(new List<string>());

            var result = controller.Add("Durian") as CreatedAtActionResult;

            Assert.NotNull(result);
            Assert.Equal("Durian", result.Value);
        }

        // 測試新增空字串，應回傳 BadRequest 並附帶錯誤訊息
        [Fact]
        public void Add_EmptyItem_ReturnsBadRequest()
        {
            var controller = new SampleController(new List<string>());

            var result = controller.Add("") as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal("Item cannot be empty", result.Value);
        }
    }
}
