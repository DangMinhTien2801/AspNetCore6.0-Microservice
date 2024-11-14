using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Product.Api.Entities;
using Product.Api.Persistence;
using Microsoft.EntityFrameworkCore;
using Product.Api.Reponsitories.Interfaces;
using AutoMapper;
using Shared.DTOs.Product;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace Product.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductReponsitory _productReponsitory;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductReponsitory productReponsitory,
            IMapper mapper, ILogger<ProductsController> logger)
        {
            _productReponsitory = productReponsitory;
            _mapper = mapper;
            _logger = logger;
        }
        private static int count = 0;
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            _logger.LogInformation("Get all product");
            var products = await _productReponsitory.GetProducts();
            var result = _mapper.Map<IEnumerable<ProductDto>>(products);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct([Required]string id)
        {
            var product = await _productReponsitory.GetProduct(id);
            if (product == null)
                return NotFound();
            var result = _mapper.Map<ProductDto>(product);
            return Ok(result);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto productDto)
        {
            var productDuplicate = await _productReponsitory.GetProductByNo(productDto.No);
            if (productDuplicate != null)
                return BadRequest($"Product No: {productDto.No} is already exist");
            var product = _mapper.Map<CatalogProduct>(productDto);
            product.Id = Guid.NewGuid().ToString();
            await _productReponsitory.CreateAsync(product);
            var result = await _productReponsitory.SaveChangeAsync();
            if(result < 1)
                return BadRequest(result);
            return StatusCode(201, result);
        }
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateProduct([Required] string id,
           [FromBody] UpdateProductDto productDto)
        {
            var product = await _productReponsitory.GetProduct(id);
            if(product == null)
                return NotFound();
            var productUpdate = _mapper.Map(productDto, product);
            await _productReponsitory.UpdateProduct(productUpdate);
            var result = await _productReponsitory.SaveChangeAsync();
            return Ok(result);
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct([Required] string id)
        {
            var product = await _productReponsitory.GetProduct(id);
            if (product == null)
                return NotFound();
            await _productReponsitory.DeleteAsync(product);
            var result = await _productReponsitory.SaveChangeAsync();
            return Ok(result);
        }
        [Authorize]
        [HttpGet("get-product-by-no/{productNo}")]
        public async Task<IActionResult> GetProductByNo([Required] string productNo)
        {
            var product = await _productReponsitory.GetProductByNo(productNo);
            if (product == null)
                return NotFound();
            var result = _mapper.Map<ProductDto>(product);
            return Ok(result);
        }
    }
}
