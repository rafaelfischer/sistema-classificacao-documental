﻿using AutoMapper;
using Prodest.Scd.Business.Model;
using Prodest.Scd.Presentation.ViewModel;

namespace Prodest.Scd.Presentation.Configuration
{
    class PresentationProfileAutoMapper : Profile
    {
        public PresentationProfileAutoMapper()
        {
            CreateMap<PlanoClassificacaoModel, PlanoClassificacaoEntidade>().ReverseMap();
            CreateMap<ItemPlanoClassificacaoModel, ItemPlanoClassificacaoEntidade>().ReverseMap();
            CreateMap<NivelClassificacaoModel, NivelClassificacaoEntidade>().ReverseMap();

        }
    }
}
