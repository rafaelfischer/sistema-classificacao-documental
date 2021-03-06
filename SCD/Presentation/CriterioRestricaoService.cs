﻿using AutoMapper;
using Prodest.Scd.Business.Base;
using Prodest.Scd.Business.Common.Exceptions;
using Prodest.Scd.Business.Model;
using Prodest.Scd.Integration.Organograma.Base;
using Prodest.Scd.Presentation.Base;
using Prodest.Scd.Presentation.ViewModel;
using Prodest.Scd.Presentation.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Prodest.Scd.Business.Model.CriterioRestricaoModel;

namespace Prodest.Scd.Presentation
{
    public class CriterioRestricaoService : ICriterioRestricaoService
    {
        private ICriterioRestricaoCore _core;
        private IPlanoClassificacaoCore _corePlanoClassificacao;
        private IDocumentoCore _coreDocumento;
        private IFundamentoLegalCore _coreFundamentoLegal;
        private IMapper _mapper;

        public CriterioRestricaoService(ICriterioRestricaoCore core, IPlanoClassificacaoCore corePlanoClassificacao, IDocumentoCore coreDocumento, IFundamentoLegalCore coreFundamentoLegal, IMapper mapper)
        {
            _corePlanoClassificacao = corePlanoClassificacao;
            _coreDocumento = coreDocumento;
            _coreFundamentoLegal = coreFundamentoLegal;
            _core = core;
            _mapper = mapper;
        }

        public async Task<CriterioRestricaoViewModel> Delete(int id)
        {
            var model = new CriterioRestricaoViewModel();
            try
            {
                await _core.DeleteAsync(id);
                model.Result = new ResultViewModel
                {
                    Ok = true,
                    Messages = new List<MessageViewModel>()
                    {
                        new MessageViewModel{
                            Message = "Item removido com sucesso!",
                            Type = TypeMessageViewModel.Success
                        }
                    }
                };
            }
            catch (ScdException e)
            {
                model.Result = new ResultViewModel
                {
                    Ok = false,
                    Messages = new List<MessageViewModel>()
                    {
                        new MessageViewModel{
                            Message = e.Message,
                            Type = TypeMessageViewModel.Fail
                        }
                    }
                };
            }
            return model;
        }

        private ICollection<EnumModel> obterListaUnidadesTempo()
        {
            return new List<EnumModel> {
                    new EnumModel { Id = (int)UnidadeTempoModel.Anos, Nome = UnidadeTempoModel.Anos.ToString() },
                    new EnumModel { Id = (int)UnidadeTempoModel.Dias, Nome = UnidadeTempoModel.Dias.ToString()  },
                    new EnumModel { Id = (int)UnidadeTempoModel.Meses, Nome = UnidadeTempoModel.Meses.ToString()  },
                    //new EnumModel { Id = (int)UnidadeTempo.Semanas, Nome = UnidadeTempo.Semanas.ToString()  },
                };
        }

        private ICollection<EnumModel> obterListaGraus()
        {
            return new List<EnumModel> {
                    new EnumModel { Id = (int)GrauSigiloModel.Reservado, Nome = "Reservado" },
                    new EnumModel { Id = (int)GrauSigiloModel.Secreto, Nome = "Secreto" },
                    new EnumModel { Id = (int)GrauSigiloModel.Ultrassecreto, Nome = "Ultrassecreto" },
                };
        }

        public async Task<CriterioRestricaoViewModel> Edit(int id)
        {
            var model = new CriterioRestricaoViewModel();
            try
            {
                model.Action = "Update";
                model.entidade = _mapper.Map<CriterioRestricaoEntidade>(await _core.SearchAsync(id));
                model.graus = obterListaGraus();
                model.unidadesTempo = obterListaUnidadesTempo();
                model.Documentos = _mapper.Map<ICollection<DocumentoEntidade>>(await _coreDocumento.SearchByPlanoAsync(model.entidade.PlanoClassificacao.Id));
                var guid = new Guid("fe88eb2a-a1f3-4cb1-a684-87317baf5a57");
                model.FundamentosLegais = _mapper.Map<ICollection<FundamentoLegalEntidade>>(await _coreFundamentoLegal.SearchAsync(guid,1,1000));
                model.Result = new ResultViewModel
                {
                    Ok = true
                };
            }
            catch (ScdException e)
            {
                model.Result = new ResultViewModel
                {
                    Ok = false,
                    Messages = new List<MessageViewModel>()
                    {
                        new MessageViewModel{
                            Message = e.Message,
                            Type = TypeMessageViewModel.Fail
                        }
                    }
                };
            }
            return model;
        }

        public async Task<CriterioRestricaoViewModel> Update(CriterioRestricaoEntidade entidade)
        {
            var model = new CriterioRestricaoViewModel();
            model.entidade = entidade;
            try
            {
                await _core.UpdateAsync(_mapper.Map<CriterioRestricaoModel>(entidade));
                model.Result = new ResultViewModel
                {
                    Ok = true,
                    Messages = new List<MessageViewModel>()
                    {
                        new MessageViewModel{
                            Message = "Item alterado com sucesso!",
                            Type = TypeMessageViewModel.Success
                        }
                    }
                };
            }
            catch (ScdException e)
            {
                model.Result = new ResultViewModel
                {
                    Ok = false,
                    Messages = new List<MessageViewModel>()
                    {
                        new MessageViewModel{
                            Message = e.Message,
                            Type = TypeMessageViewModel.Fail
                        }
                    }
                };
            }
            return model;
        }

        public async Task<CriterioRestricaoViewModel> Create(CriterioRestricaoEntidade entidade)
        {
            var model = new CriterioRestricaoViewModel();
            try
            {
                var modelInsert = await _core.InsertAsync(_mapper.Map<CriterioRestricaoModel>(entidade));
                model.entidade = _mapper.Map<CriterioRestricaoEntidade>(modelInsert);
                model.Result = new ResultViewModel
                {
                    Ok = true,
                    Messages = new List<MessageViewModel>()
                    {
                        new MessageViewModel{
                            Message = "Item criado com sucesso!",
                            Type = TypeMessageViewModel.Success
                        }
                    }
                };
            }
            catch (ScdException e)
            {
                //Necessário configurar a tela para continuar a ação 
                model.Action = "Create";
                model.graus = obterListaGraus();
                model.unidadesTempo = obterListaUnidadesTempo();
                model.entidade = entidade;
                model.Result = new ResultViewModel
                {
                    Ok = false,
                    Messages = new List<MessageViewModel>()
                    {
                        new MessageViewModel{
                            Message = e.Message,
                            Type = TypeMessageViewModel.Fail
                        }
                    }
                };
            }
            return model;
        }




        public async Task<CriterioRestricaoViewModel> Search(FiltroCriterioRestricao filtro)
        {
            var model = new CriterioRestricaoViewModel();
            model.plano = new PlanoClassificacaoEntidade { Id = filtro.IdPlanoClassificacao };
            var entidades = await _core.SearchByPlanoClassificacaoAsync(filtro.IdPlanoClassificacao);
            model.entidades = _mapper.Map<ICollection<CriterioRestricaoEntidade>>(entidades);
            model.Result = new ResultViewModel
            {
                Ok = true
            };
            return model;
        }

        public async Task<CriterioRestricaoViewModel> New(int idPlanoClassificacao)
        {
            var model = new CriterioRestricaoViewModel();
            try
            {
                model.Action = "Create";
                model.entidade = new CriterioRestricaoEntidade
                {
                    PlanoClassificacao = new PlanoClassificacaoEntidade { Id = idPlanoClassificacao },
                    Documentos = new List<DocumentoEntidade>()
                };
                var guid = new Guid("fe88eb2a-a1f3-4cb1-a684-87317baf5a57");
                model.FundamentosLegais = _mapper.Map<ICollection<FundamentoLegalEntidade>>(await _coreFundamentoLegal.SearchAsync(guid, 1, 1000));
                model.Documentos = _mapper.Map<ICollection<DocumentoEntidade>>(await _coreDocumento.SearchByPlanoAsync(idPlanoClassificacao));
                model.graus = obterListaGraus();
                model.unidadesTempo = obterListaUnidadesTempo();
                model.Result = new ResultViewModel
                {
                    Ok = true
                };
            }
            catch (ScdException e)
            {
                model.Result = new ResultViewModel
                {
                    Ok = false,
                    Messages = new List<MessageViewModel>()
                    {
                        new MessageViewModel{
                            Message = e.Message,
                            Type = TypeMessageViewModel.Fail
                        }
                    }
                };
            }
            return model;
        }




    }
}
