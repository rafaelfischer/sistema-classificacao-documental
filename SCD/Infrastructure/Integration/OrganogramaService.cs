﻿using Prodest.Scd.Infrastructure.Integration.Common;
using Prodest.Scd.Integration.Common.Base;
using Prodest.Scd.Integration.Organograma.Base;
using Prodest.Scd.Integration.Organograma.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prodest.Scd.Infrastructure.Integration
{
    public class OrganogramaService : IOrganogramaService
    {
        private IClientAccessTokenProvider _clientAccessToken;

        public OrganogramaService(IClientAccessTokenProvider clientAccessToken)
        {
            _clientAccessToken = clientAccessToken;
        }

        public async Task<List<OrganogramaOrganizacao>> SearchAsync()
        {
            string url = $"https://sistemas.es.gov.br/prodest/organograma/api/organizacoes/{Environment.GetEnvironmentVariable("GuidGEES")}/filhas";

            List<OrganogramaOrganizacao> organizacoes = await JsonData.DownloadAsync<List<OrganogramaOrganizacao>>(url, _clientAccessToken.AccessToken);

            return organizacoes;
        }

        public async Task<OrganogramaOrganizacao> SearchAsync(string guid)
        {
            string url = $"https://sistemas.es.gov.br/prodest/organograma/api/organizacoes/{guid}";

            OrganogramaOrganizacao organizacao = await JsonData.DownloadAsync<OrganogramaOrganizacao>(url, _clientAccessToken.AccessToken);

            return organizacao;
        }

        public async Task<OrganogramaOrganizacao> SearchPatriarcaAsync(string guid)
        {
            string url = $"https://sistemas.es.gov.br/prodest/organograma/api/organizacoes/{guid}/patriarca";

            OrganogramaOrganizacao organizacao = await JsonData.DownloadAsync<OrganogramaOrganizacao>(url, _clientAccessToken.AccessToken);

            return organizacao;
        }
    }
}
