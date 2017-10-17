﻿using Prodest.Scd.Business.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Prodest.Scd.Business.Base
{
    public interface IItemPlanoClassificacaoCore
    {
        Task<ItemPlanoClassificacaoModel> InsertAsync(ItemPlanoClassificacaoModel itemPlanoClassificacao);

        Task<ItemPlanoClassificacaoModel> Search(int id);

        Task<ICollection<ItemPlanoClassificacaoModel>> Search(int idPlanoClassificacao, int page, int count);

        Task<int> CountAsync(int idPlanoClassificacao);

        Task UpdateAsync(ItemPlanoClassificacaoModel itemPlanoClassificacao);

        Task DeleteAsync(int id);
    }
}