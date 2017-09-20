﻿using AutoMapper;
using Prodest.Scd.Business.Base;
using Prodest.Scd.Business.Model;
using Prodest.Scd.Business.Validation;
using Prodest.Scd.Persistence.Base;
using Prodest.Scd.Persistence.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prodest.Scd.Business
{
    public class NivelClassificacaoCore : INivelClassificacaoCore
    {
        private IUnitOfWork _unitOfWork;
        private IGenericRepository<NivelClassificacao> _niveisClassificacao;
        private NivelClassificacaoValidation _validation;
        private IMapper _mapper;
        private IOrganizacaoCore _organizacaoCore;

        public NivelClassificacaoCore(IScdRepositories repositories, NivelClassificacaoValidation validation, IMapper mapper, IOrganizacaoCore organizacaoCore)
        {
            _unitOfWork = repositories.UnitOfWork;
            _niveisClassificacao = repositories.NiveisClassificacao;
            _validation = validation;
            _mapper = mapper;
            _organizacaoCore = organizacaoCore;
        }

        public int Count(Guid guidOrganizacao)
        {
            _validation.OrganizacaoValid(guidOrganizacao);

            var count = _niveisClassificacao.Where(pc => pc.Organizacao.GuidOrganizacao.Equals(guidOrganizacao))
                                            .Count();

            return count;
        }

        public async Task DeleteAsync(int id)
        {
            NivelClassificacao nivelClassificacao = SearchPersistence(id);

            _validation.CanDelete(nivelClassificacao);

            _niveisClassificacao.Remove(nivelClassificacao);

            await _unitOfWork.SaveAsync();
        }

        public async Task<NivelClassificacaoModel> InsertAsync(NivelClassificacaoModel nivelClassificacaoModel)
        {

            _validation.BasicValid(nivelClassificacaoModel);

            _validation.IdInsertValid(nivelClassificacaoModel.Id);

            //TODO: Retirar este trecho quando o sistema conseguir obter organzação do usuário
            Guid guidProdest = new Guid(Environment.GetEnvironmentVariable("guidGEES"));
            OrganizacaoModel organizacao = _organizacaoCore.SearchAsync(guidProdest);

            nivelClassificacaoModel.Organizacao = organizacao;

            NivelClassificacao nivelClassificacao = _mapper.Map<NivelClassificacao>(nivelClassificacaoModel);
            nivelClassificacao.Ativo = true;

            await _niveisClassificacao.AddAsync(nivelClassificacao);

            await _unitOfWork.SaveAsync();

            nivelClassificacaoModel = _mapper.Map<NivelClassificacaoModel>(nivelClassificacao);

            return nivelClassificacaoModel;
        }

        public NivelClassificacaoModel Search(int id)
        {
            NivelClassificacao nivelClassificacao = SearchPersistence(id);

            NivelClassificacaoModel nivelClassificacaoModel = _mapper.Map<NivelClassificacaoModel>(nivelClassificacao);

            return nivelClassificacaoModel;
        }

        public List<NivelClassificacaoModel> Search(Guid guidOrganizacao, int page, int count)
        {
            _validation.OrganizacaoValid(guidOrganizacao);

            _validation.PaginationSearch(page, count);

            int skip = (page - 1) * count;

            List<NivelClassificacao> nivelsClassificacao = _niveisClassificacao.Where(pc => pc.Organizacao.GuidOrganizacao.Equals(guidOrganizacao))
                                                                               .OrderBy(pc => pc.Ativo)
                                                                               .ThenBy(pc => pc.Descricao)
                                                                               .Skip(skip)
                                                                               .Take(count)
                                                                               .ToList();
            List<NivelClassificacaoModel> nivelsClassificacaoModel = _mapper.Map<List<NivelClassificacaoModel>>(nivelsClassificacao);

            return nivelsClassificacaoModel;
        }

        public async Task UpdateAsync(NivelClassificacaoModel nivelClassificacaoModel)
        {
            _validation.Valid(nivelClassificacaoModel);

            NivelClassificacao nivelClassificacao = SearchPersistence(nivelClassificacaoModel.Id);

            _validation.CanUpdate(nivelClassificacaoModel, nivelClassificacao);

            _mapper.Map(nivelClassificacaoModel, nivelClassificacao);

            await _unitOfWork.SaveAsync();
        }

        private NivelClassificacao SearchPersistence(int id)
        {
            _validation.IdValid(id);

            NivelClassificacao nivelClassificacao = _niveisClassificacao.Where(pc => pc.Id == id)
                                                                        .SingleOrDefault();

            _validation.Found(nivelClassificacao);

            return nivelClassificacao;
        }
    }
}