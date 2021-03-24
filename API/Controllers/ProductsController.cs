using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _productsBrandRepo;
        private readonly IMapper _mapper;
        public ProductsController(IGenericRepository<Product> productsRep,
        IGenericRepository<ProductType> productTypeRepo,
        IGenericRepository<ProductBrand> productsBrandRepo, IMapper mapper)
        {
            _mapper = mapper;
            _productsBrandRepo = productsBrandRepo;
            _productTypeRepo = productTypeRepo;
            _productsRepo = productsRep;

        }
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductsToReturnDto>>> GetProducts()
    {
        var spec = new ProductsWithTypeAndBrandsSpecification();
        var products = await _productsRepo.ListAsync(spec);
        return Ok(
            _mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductsToReturnDto>>(products));
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductsToReturnDto>> GetProduct(int id)
    {
        var spec = new ProductsWithTypeAndBrandsSpecification(id);
        var product = await _productsRepo.GetEntityWithSpec(spec);
        return _mapper.Map<Product,ProductsToReturnDto>(product);
    }
    [HttpGet("brands")]
    public async Task<ActionResult<List<Product>>> GetProductBrands()
    {
        var products = await _productsBrandRepo.ListAllAsync();

        return Ok(products);
    }
    [HttpGet("types")]
    public async Task<ActionResult<List<Product>>> GetProductTypes()
    {
        var products = await _productTypeRepo.ListAllAsync();
        return Ok(products);
    }
}
}