﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Prodest.Scd.Business.Base;
using Prodest.Scd.Business.Model;
using Prodest.Scd.Business.Validation;
using Prodest.Scd.Integration.Organograma;
using Prodest.Scd.Integration.Organograma.Model;
using Prodest.Scd.Persistence.Base;
using Prodest.Scd.Persistence.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prodest.Scd.Business
{
    public class PlanoClassificacaoCore : IPlanoClassificacaoCore
    {
        private IUnitOfWork _unitOfWork;
        private IGenericRepository<PlanoClassificacao> _planosClassificacao;
        private PlanoClassificacaoValidation _validation;
        private IMapper _mapper;
        private OrganogramaService _organogramaService;
        private IOrganizacaoCore _organizacaoCore;

        public PlanoClassificacaoCore(IScdRepositories repositories, PlanoClassificacaoValidation validation, IMapper mapper, OrganogramaService organogramaService, IOrganizacaoCore organizacaoCore)
        {
            _unitOfWork = repositories.UnitOfWork;
            _planosClassificacao = repositories.PlanosClassificacao;
            _validation = validation;
            _mapper = mapper;
            _organogramaService = organogramaService;
            _organizacaoCore = organizacaoCore;
        }

        public async Task<int> CountAsync(string guidOrganizacao)
        {
            _validation.OrganizacaoFilled(guidOrganizacao);
            _validation.OrganizacaoValid(guidOrganizacao);

            Guid guid = new Guid(guidOrganizacao);

            var count = await _planosClassificacao.Where(pc => pc.GuidOrganizacao.Equals(guid))
                                                  .CountAsync();

            return count;
        }

        public async Task DeleteAsync(int id)
        {
            PlanoClassificacao planoClassificacao = await SearchPersistenceAsync(id);

            _planosClassificacao.Remove(planoClassificacao);

            await _unitOfWork.SaveAsync();
        }

        public async Task<PlanoClassificacaoModel> InsertAsync(PlanoClassificacaoModel planoClassificacaoModel)
        {
            _validation.BasicValid(planoClassificacaoModel);

            OrganogramaOrganizacao organogramaOrganizacaoPatriarca = await _organogramaService.SearchPatriarcaAsync(planoClassificacaoModel.GuidOrganizacao);

            OrganizacaoModel organizacaoPatriarca = await _organizacaoCore.SearchAsync(organogramaOrganizacaoPatriarca.Guid.ToString());

            planoClassificacaoModel.Organizacao = organizacaoPatriarca;

            PlanoClassificacao planoClassificacao = _mapper.Map<PlanoClassificacao>(planoClassificacaoModel);

            await _planosClassificacao.AddAsync(planoClassificacao);

            await _unitOfWork.SaveAsync();

            planoClassificacaoModel = _mapper.Map<PlanoClassificacaoModel>(planoClassificacao);

            return planoClassificacaoModel;
        }

        public async Task<PlanoClassificacaoModel> SearchAsync(int id)
        {
            PlanoClassificacao planoClassificacao = await SearchPersistenceAsync(id);

            PlanoClassificacaoModel planoClassificacaoModel = _mapper.Map<PlanoClassificacaoModel>(planoClassificacao);

            return planoClassificacaoModel;
        }

        public async Task<List<PlanoClassificacaoModel>> SearchAsync(string guidOrganizacao)
        {
            _validation.OrganizacaoFilled(guidOrganizacao);
            _validation.OrganizacaoValid(guidOrganizacao);

            Guid guid = new Guid(guidOrganizacao);

            List<PlanoClassificacao> planosClassificacao = await _planosClassificacao.Where(pc => pc.GuidOrganizacao.Equals(guid))
                                                                                     .OrderBy(pc => pc.Codigo)
                                                                                     .OrderBy(pc => pc.Descricao)
                                                                                     .OrderBy(pc => pc.InicioVigencia)
                                                                                     .OrderBy(pc => pc.FimVigencia)
                                                                                     .ToListAsync();

            List<PlanoClassificacaoModel> planosClassificacaoModel = _mapper.Map<List<PlanoClassificacaoModel>>(planosClassificacao);

            return planosClassificacaoModel;
        }

        public async Task<List<PlanoClassificacaoModel>> SearchAsync(string guidOrganizacao, int page, int count)
        {
            _validation.OrganizacaoFilled(guidOrganizacao);
            _validation.OrganizacaoValid(guidOrganizacao);

            _validation.PaginationSearch(page, count);

            Guid guid = new Guid(guidOrganizacao);
            int skip = page * count;

            List<PlanoClassificacao> planosClassificacao = await _planosClassificacao.Where(pc => pc.GuidOrganizacao.Equals(guid))
                                                                                     .OrderBy(pc => pc.Codigo)
                                                                                     .OrderBy(pc => pc.Descricao)
                                                                                     .OrderBy(pc => pc.InicioVigencia)
                                                                                     .OrderBy(pc => pc.FimVigencia)
                                                                                     .Skip(skip)
                                                                                     .Take(count)
                                                                                     .ToListAsync();

            List<PlanoClassificacaoModel> planosClassificacaoModel = _mapper.Map<List<PlanoClassificacaoModel>>(planosClassificacao);

            return planosClassificacaoModel;
        }

        public async Task UpdateAsync(PlanoClassificacaoModel planoClassificacaoModel)
        {
            _validation.Valid(planoClassificacaoModel);

            PlanoClassificacao planoClassificacao = await SearchPersistenceAsync(planoClassificacaoModel.Id);

            _validation.CanUpDate(planoClassificacao);

            if (!planoClassificacaoModel.GuidOrganizacao.ToUpper().Equals(planoClassificacao.GuidOrganizacao.ToString().ToUpper()))
            {
                OrganogramaOrganizacao organogramaOrganizacaoPatriarca = await _organogramaService.SearchPatriarcaAsync(planoClassificacaoModel.GuidOrganizacao);

                OrganizacaoModel organizacaoPatriarca = await _organizacaoCore.SearchAsync(organogramaOrganizacaoPatriarca.Guid.ToString());

                if (planoClassificacao.IdOrganizacao != organizacaoPatriarca.Id)
                    planoClassificacaoModel.Organizacao = organizacaoPatriarca;
            }

            _mapper.Map(planoClassificacaoModel, planoClassificacao);

            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateFimVigenciaAsync(int id, DateTime fimVigencia)
        {
            PlanoClassificacao planoClassificacao = await SearchPersistenceAsync(id);

            _validation.FimVigenciaValid(fimVigencia, planoClassificacao.InicioVigencia);

            planoClassificacao.FimVigencia = fimVigencia;

            await _unitOfWork.SaveAsync();
        }

        private async Task<PlanoClassificacao> SearchPersistenceAsync(int id)
        {
            _validation.IdValid(id);

            PlanoClassificacao planoClassificacao = await _planosClassificacao.Where(pc => pc.Id == id)
                                                                              .SingleOrDefaultAsync();

            _validation.Found(planoClassificacao);

            return planoClassificacao;
        }
    }
}