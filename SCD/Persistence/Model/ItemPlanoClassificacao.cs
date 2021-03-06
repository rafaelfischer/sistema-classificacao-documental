﻿using System.Collections.Generic;

namespace Prodest.Scd.Persistence.Model
{
    public partial class ItemPlanoClassificacao
    {
        public ItemPlanoClassificacao()
        {
            Documentos = new HashSet<Documento>();
            ItensPlanoClassificacaoChildren = new HashSet<ItemPlanoClassificacao>();
        }

        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public int IdPlanoClassificacao { get; set; }
        public int IdNivelClassificacao { get; set; }
        public int? IdItemPlanoClassificacaoParent { get; set; }

        public ItemPlanoClassificacao ItemPlanoClassificacaoParent { get; set; }
        public NivelClassificacao NivelClassificacao { get; set; }
        public PlanoClassificacao PlanoClassificacao { get; set; }
        public ICollection<Documento> Documentos { get; set; }
        public ICollection<ItemPlanoClassificacao> ItensPlanoClassificacaoChildren { get; set; }
    }
}
