﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Prodest.Scd.Business.Model
{
    public class TermoClassificacaoInformacaoModel
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public Guid GuidOrganizacao { get; set; }
        public GrauSigiloModel GrauSigilo
        {
            get
            {
                GrauSigiloModel grauSigilo = default(GrauSigiloModel);

                if (FimPrazoSigilo <= DataProducaoDocumento.AddYears(5))
                    grauSigilo = GrauSigiloModel.Reservado;
                else if (FimPrazoSigilo <= DataProducaoDocumento.AddYears(15))
                    grauSigilo = GrauSigiloModel.Secreto;
                else if (FimPrazoSigilo <= DataProducaoDocumento.AddYears(25))
                    grauSigilo = GrauSigiloModel.Ultrassecreto;

                return grauSigilo;
            }
        }
        public DateTime FimPrazoSigilo
        {
            get
            {
                DateTime fimPrazoSigilo = default(DateTime);

                if (UnidadeTempoModel.Anos.Equals(UnidadePrazoSigilo))
                    fimPrazoSigilo = DataProducaoDocumento.AddYears(PrazoSigilo);
                else if (UnidadeTempoModel.Meses.Equals(UnidadePrazoSigilo))
                    fimPrazoSigilo = DataProducaoDocumento.AddMonths(PrazoSigilo);
                else if (UnidadeTempoModel.Dias.Equals(UnidadePrazoSigilo))
                    fimPrazoSigilo = DataProducaoDocumento.AddDays(PrazoSigilo);
                //else if (UnidadeTempo.Semanas.Equals(UnidadePrazoSigilo))
                //    fimPrazoSigilo = DataProducaoDocumento.AddDays(PrazoSigilo * 7);

                return fimPrazoSigilo;
            }
        }
        public TipoSigiloModel TipoSigilo { get; set; }
        public string ConteudoSigilo { get; set; }
        public string IdentificadorDocumento { get; set; }
        public DateTime DataProducaoDocumento { get; set; }
        public string FundamentoLegal { get; set; }
        public string Justificativa { get; set; }
        public DateTime DataClassificacao { get; set; }
        public string CpfUsuario { get; set; }
        public string CpfIndicacaoAprovador { get; set; }
        public int PrazoSigilo { get; set; }
        public UnidadeTempoModel UnidadePrazoSigilo { get; set; }
        public StatusTermoClassificacaoInformacaoModel? Status { get; set; }

        public DocumentoModel Documento { get; set; }
        public CriterioRestricaoModel CriterioRestricao { get; set; }

        public enum TipoSigiloModel
        {
            Parcial = 1,
            Total = 2
        }

        public enum StatusTermoClassificacaoInformacaoModel
        {
            Solicitado = 1,
            Cancelado = 2,
            Aprovado = 3,
            Reprovado = 4,
            Reclassificado = 5,
            Desclassificado = 6
        }
    }
}