﻿using AutoMapper;
using Prodest.Scd.Business.Model;
using Prodest.Scd.Persistence.Model;

namespace Prodest.Scd.Infrastructure.Configuration
{
    public class InfrastructureProfileAutoMapper : Profile
    {
        public InfrastructureProfileAutoMapper()
        {
            #region Item do Plano de Classificação
            CreateMap<ItemPlanoClassificacao, ItemPlanoClassificacaoModel>().ReverseMap();
            #endregion

            #region Nível de Classificação
            CreateMap<NivelClassificacao, NivelClassificacaoModel>();

            CreateMap<NivelClassificacaoModel, NivelClassificacao>()
                .ForMember(dest => dest.Organizacao,
                opt =>
                {
                    opt.Condition((src, dest, srcMember, destMember) =>
                    {
                        return (destMember == null);
                    });
                })
                .ForMember(dest => dest.IdOrganizacao, opt => opt.MapFrom(src => src.Organizacao != null ? src.Organizacao.Id : default(int)));
            #endregion

            #region Organização
            CreateMap<Organizacao, OrganizacaoModel>();

            CreateMap<OrganizacaoModel, Organizacao>()
                .ForMember(dest => dest.PlanosClassificacao, opt => opt.Ignore());
            #endregion

            #region Plano de Classificação
            CreateMap<PlanoClassificacao, PlanoClassificacaoModel>()
                .MaxDepth(1)
            ;

            CreateMap<PlanoClassificacaoModel, PlanoClassificacao>()
                .ForMember(dest => dest.Id, opt =>
                {
                    opt.Condition((src, dest, srcMember, destMember) =>
                    {
                        return (destMember == default(int));
                    });
                })
                .ForMember(dest => dest.IdOrganizacao, opt => opt.MapFrom(src => src.Organizacao != null ? src.Organizacao.Id : default(int)))
                .ForMember(dest => dest.Organizacao, opt => opt.Ignore())
                .ForMember(dest => dest.ItensPlanoClassificacao, opt => opt.Ignore());
            #endregion

            #region Tipo Documental
            CreateMap<TipoDocumental, TipoDocumentalModel>();

            CreateMap<TipoDocumentalModel, TipoDocumental>()
                .ForMember(dest => dest.Id, opt =>
                {
                    opt.Condition((src, dest, srcMember, destMember) =>
                    {
                        return (destMember == default(int));
                    });
                })
                .ForMember(dest => dest.Organizacao, opt => opt.Ignore())
                .ForMember(dest => dest.IdOrganizacao, opt => opt.MapFrom(src => src.Organizacao != null ? src.Organizacao.Id : default(int)));
            #endregion
        }
    }
}