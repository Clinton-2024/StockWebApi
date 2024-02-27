using AutoMapper;
using StockWebApi.Core.Dtos.Category;
using StockWebApi.Core.Dtos.Inventaire;
using StockWebApi.Core.Dtos.Marchandise;
using StockWebApi.Core.Dtos.Operation;
using StockWebApi.Core.Dtos.OtherObjects;
using StockWebApi.Core.Dtos.Traitement;
using StockWebApi.Core.Entities;
using System.Linq;

namespace Backend.Core.AutoMapperConfig
{
    public class AutoMapperConfigProfile : Profile
    {
        public AutoMapperConfigProfile()
        {
            // Category
                CreateMap<CategoryCreateDto, Category>();
                CreateMap<Category, CategoryGetAllDto>().ForMember(dest => dest.NombreArticles, opt => opt.MapFrom(src => src.Marchandises.Count())); 
                CreateMap<CategoryGetAllDto , Category>();


            // Marchandise
            CreateMap<MarchandiseEditDto, Marchandise>();
            CreateMap<MarchandiseCreateDto, Marchandise>(); 
                CreateMap<Marchandise, MarchandiseGetDto>().ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.StockInitial * src.PrixUnitaire));
            CreateMap<Marchandise, MarchandiseGetAllDto>().ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.StockInitial * src.PrixUnitaire));

           // Operation
            
            CreateMap<OperationEntrieCreateDto, Operation>();
            CreateMap<OperationOutPutCreateDto, Operation>();
            CreateMap<Operation, OperationGetAllDto>()
                .ForMember(dest => dest.Designation, opt => opt.MapFrom(src => src.Marchandise.Designation))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Marchandise.Currency))
                .ForMember(dest => dest.Unite, opt => opt.MapFrom(src => src.Marchandise.Unite))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Marchandise.Category.Name))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Quantity * src.Price));

            // Inventaire

            CreateMap<Marchandise, InventaireGetAllDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.Valeur, opt => 
                    opt.MapFrom( src => (( src.StockInitial + src.Operations.Where(op => op.Type == StaticTypeOperation.Entrie).Sum(op => op.Quantity))
                    - src.Operations.Where(op => op.Type == StaticTypeOperation.OutPut).Sum(op => op.Quantity)) 
                    * (((src.StockInitial * src.PrixUnitaire) + (src.Operations.Where(opt => opt.Type == StaticTypeOperation.Entrie).Select(opt => opt.Quantity * opt.Price).Sum())) / (src.StockInitial + src.Operations.Where(opt => opt.Type == StaticTypeOperation.Entrie).Select(opt => opt.Quantity).Sum())) ))
                .ForMember(dest => dest.Statut,opt => opt.MapFrom( src => ((src.StockInitial + src.Operations.Where(op => op.Type == StaticTypeOperation.Entrie).Sum(op => op.Quantity)) - src.Operations.Where(op => op.Type == StaticTypeOperation.OutPut).Sum(op => op.Quantity))
                    > src.SeuilAlerte ? StaticStatutInventaire.Normal :
                    ((src.StockInitial + src.Operations.Where(op => op.Type == StaticTypeOperation.Entrie).Sum(op => op.Quantity)) - (src.Operations.Where(op => op.Type == StaticTypeOperation.OutPut).Sum(op => op.Quantity))
                    == 0? StaticStatutInventaire.Disponible : StaticStatutInventaire.Faible)))
                .ForMember(dest => dest.StockFinal, opt => opt.MapFrom(src => (src.StockInitial + src.Operations.Where(op => op.Type == StaticTypeOperation.Entrie).Sum(op => op.Quantity)) - src.Operations.Where(op => op.Type == StaticTypeOperation.OutPut).Sum(op => op.Quantity) ))
                .ForMember(dest => dest.Entries, opt => opt.MapFrom(src => src.Operations.Where(op => op.Type == StaticTypeOperation.Entrie).Sum(op => op.Quantity)))
                .ForMember(dest => dest.Output, opt => opt.MapFrom(src => src.Operations.Where(op => op.Type == StaticTypeOperation.OutPut).Sum(op => op.Quantity)  ))
                .ForMember(dest => dest.CUMP, opt => opt.MapFrom(src => ((src.StockInitial * src.PrixUnitaire) + (src.Operations.Where(opt => opt.Type == StaticTypeOperation.Entrie).Select(opt => opt.Quantity * opt.Price).Sum())) / (src.StockInitial + src.Operations.Where(opt => opt.Type == StaticTypeOperation.Entrie).Select(opt => opt.Quantity).Sum()) ));

            // Traitement

            CreateMap<Marchandise, TraitementStockTotal>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Category.Id))
                .ForMember(dest => dest.StockTotal, opt => opt.MapFrom(src => (src.StockInitial + src.Operations.Where(op => op.Type == StaticTypeOperation.Entrie).Sum(op => op.Quantity)) - src.Operations.Where(op => op.Type == StaticTypeOperation.OutPut).Sum(op => op.Quantity)))
                .ForMember(dest => dest.ValeurStock, opt =>
                    opt.MapFrom(src => ((src.StockInitial + src.Operations.Where(op => op.Type == StaticTypeOperation.Entrie).Sum(op => op.Quantity))
                    - src.Operations.Where(op => op.Type == StaticTypeOperation.OutPut).Sum(op => op.Quantity))
                    * (((src.StockInitial * src.PrixUnitaire) + (src.Operations.Where(opt => opt.Type == StaticTypeOperation.Entrie).Select(opt => opt.Quantity * opt.Price).Sum())) / (src.StockInitial + src.Operations.Where(opt => opt.Type == StaticTypeOperation.Entrie).Select(opt => opt.Quantity).Sum()))));

            // TraitementEntrie

            CreateMap<Marchandise, TraitementEntries>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Category.Id))
                .ForMember(dest => dest.TotalEntries, opt => opt.MapFrom(src => src.Operations.Where(opt => opt.Type == StaticTypeOperation.Entrie).Sum(opt => opt.Quantity)))
                .ForMember(dest => dest.ValeurEntries, opt => opt.MapFrom(src => src.Operations.Where(opt => opt.Type == StaticTypeOperation.Entrie).Select(opt => opt.Quantity * opt.Price).Sum()));

            // TraitementOutput 

            CreateMap<Marchandise, TraitementOutput>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Category.Id))
                .ForMember(dest => dest.TotalOutput, opt => opt.MapFrom(src => src.Operations.Where(opt => opt.Type == StaticTypeOperation.OutPut).Sum(opt => opt.Quantity)))
                .ForMember(dest => dest.ValeurOutput, opt => opt.MapFrom(src => src.Operations.Where(opt => opt.Type == StaticTypeOperation.OutPut).Select(opt => opt.Quantity * opt.Price).Sum()));

            // Repartition

            CreateMap<Category, RepartitionStock>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Marchandises.Sum(m => m.StockInitial) + src.Marchandises.Sum(m => m.Operations.Where(o => o.Type == StaticTypeOperation.Entrie).Sum(o => o.Quantity)) - src.Marchandises.Sum(m => m.Operations.Where(o => o.Type == StaticTypeOperation.OutPut).Sum(o => o.Quantity))));
        }
    }
}
