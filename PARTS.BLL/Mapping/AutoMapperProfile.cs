using AutoMapper;
using PARTS.BLL.DTOs.Requests;
using PARTS.BLL.DTOs.Responses;
using PARTS.DAL.Entities;
using PARTS.DAL.Entities.Item;
using PARTS.DAL.Entities.Vehicle;

namespace PARTS.BLL.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateBrandMap();
            CreateCategoryImagerMap();
            CreateCategotyMap();
            CreateEngineMap();
            CreateMakeMap();
            CreateModelMap();
            CreatePartImageMap();
            CreatePartMap();
            CreateSubModelMap();
            CreateVehicleMap();
            CreateOrderMap();

        }


        private void CreateBrandMap()
        {
            CreateMap<Brand, BrandRequest>().ReverseMap();
            CreateMap<Brand, BrandResponse>().ReverseMap();

        }
        private void CreateCategoryImagerMap()
        {
            CreateMap<CategoryImage, CategoryImageRequest>().ReverseMap();
            CreateMap<CategoryImage, CategoryImageResponse>().ReverseMap();

        }


        private void CreateCategotyMap()
        {
            CreateMap<Category, CategoryRequest>().ReverseMap();
            CreateMap<Category, CategoryResponse>().ReverseMap();

        }

        private void CreateEngineMap()
        {
            CreateMap<Engine, EngineRequest>().ReverseMap();
            CreateMap<Engine, EngineResponse>().ReverseMap();

        }

        private void CreateMakeMap()
        {
            CreateMap<Make, MakeRequest>().ReverseMap();
            CreateMap<Make, MakeResponse>().ReverseMap();

        }

        private void CreateModelMap()
        {
            CreateMap<Model, ModelRequest>().ReverseMap();
            CreateMap<Model, ModelResponse>().ReverseMap();

        }

        private void CreatePartImageMap()
        {
            CreateMap<PartImage, PartImageRequest>().ReverseMap();
            CreateMap<PartImage, PartImageResponse>().ReverseMap();

        }

        private void CreatePartMap()
        {
            CreateMap<Part, PartRequest>().ReverseMap();
            CreateMap<Part, PartResponse>().ReverseMap();

        }

        private void CreateSubModelMap()
        {
            CreateMap<SubModel, SubModelRequest>().ReverseMap();
            CreateMap<SubModel, SubModelResponse>().ReverseMap();

        }

        private void CreateVehicleMap()
        {
            CreateMap<Vehicle, VehicleRequest>().ReverseMap();
            CreateMap<Vehicle, VehicleResponse>().ReverseMap();

        }
        private void CreateOrderMap()
        {
            CreateMap<Order, OrderRequest>().ReverseMap();
            CreateMap<Order, OrderResponse>().ReverseMap();

        }


    }
}
