﻿using Prodest.Scd.Business.Common.Exceptions;
using Prodest.Scd.Business.Model;
using Prodest.Scd.Business.Validation.Common;

namespace Prodest.Scd.Business.Validation
{
    public class TipoDocumentalValidation : CommonValidation
    {
        #region Valid
        internal void Valid(TipoDocumentalModel tipoDocumental)
        {
            BasicValid(tipoDocumental);

            IdValid(tipoDocumental.Id);
        }
        #endregion

        #region Basic Valid
        internal void BasicValid(TipoDocumentalModel tipoDocumental)
        {
            NotNull(tipoDocumental);

            Filled(tipoDocumental);

            //OrganizacaoNotNull(tipoDocumental.Organizacao);
        }

        private void NotNull(TipoDocumentalModel tipoDocumental)
        {
            if (tipoDocumental == null)
                throw new ScdException("O Tipo Documental não pode ser nulo.");
        }

        #region Filled
        internal void Filled(TipoDocumentalModel tipoDocumental)
        {
            DescricaoFilled(tipoDocumental.Descricao);
        }

        private void DescricaoFilled(string descricao)
        {
            if (string.IsNullOrWhiteSpace(descricao) || string.IsNullOrWhiteSpace(descricao.Trim()))
                throw new ScdException("A descrição não pode ser vazia ou nula.");
        }

        //private void OrganizacaoNotNull(OrganizacaoModel organizacao)
        //{
        //    if (organizacao == null)
        //        throw new ScdException("A organização não pode ser nula.");
        //}
        #endregion
        #endregion

        internal void Found(TipoDocumentalModel tipoDocumentalModel)
        {
            if (tipoDocumentalModel == null)
                throw new ScdException("Tipo Documental não encontrado.");
        }
        internal void CanDelete(TipoDocumentalModel tipoDocumentalModel)
        {
            //TODO: Após a inserção de Itens de Plano de Classificação verificar se existem algum associado
            //if (tipoDocumental.ItensPlanoClassificacao != null && tipoDocumental.ItensPlanoClassificacao.Count > 0)
            //    throw new ScdException("O Nivel de Classificação possui itens e não pode ser excluído.");
        }

        internal void CanUpdate(TipoDocumentalModel newTipoDocumentalModel, TipoDocumentalModel oldTipoDocumentalModel)
        {
            //TODO: Verificar após desacoplar o repositório do Negócio
            if (newTipoDocumentalModel.Organizacao != null /*&& (oldTipoDocumental.Organizacao.Id != newTipoDocumentalModel.Organizacao.Id || !oldTipoDocumental.Organizacao.GuidOrganizacao.Equals(newTipoDocumentalModel.Organizacao.GuidOrganizacao))*/)
            {
                throw new ScdException("Não é possível atualizar a Organização do Tipo Documental.");
            }
        }
    }
}