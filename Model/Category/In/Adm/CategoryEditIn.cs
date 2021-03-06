﻿using Model.VM;
using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class CategoryEditIn
    {
        [Display(Name = "Sequence", ResourceType = typeof(i18n.Resource))]
        public int CategoryId { get; set; }
        
        [Display(Name = "Parent", ResourceType = typeof(i18n.Resource))]
        public string Parent { get; set; }        

        [Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
        public string Code { get; set; }

        [Display(Name = "Category", ResourceType = typeof(i18n.Resource))]
        public string Name { get; set; }

        [Display(Name = "PB", ResourceType = typeof(i18n.Resource))]
        public bool pb { get; set; }

        [Display(Name = "PBEmbarked", ResourceType = typeof(i18n.Resource))]
        public bool pbEmbarked { get; set; }

        [Display(Name = "Release", ResourceType = typeof(i18n.Resource))]
        public bool Release { get; set; }


        [Display(Name = "Identifier", ResourceType = typeof(i18n.Resource))]
        public bool ShowIdentifier { get; set; }

        [Display(Name = "Competence", ResourceType = typeof(i18n.Resource))]
        public bool ShowCompetence { get; set; }

        [Display(Name = "Validity", ResourceType = typeof(i18n.Resource))]
        public bool ShowValidity { get; set; }

        [Display(Name = "DocumentView", ResourceType = typeof(i18n.Resource))]
        public bool ShowDocumentView { get; set; }

        [Display(Name = "Note", ResourceType = typeof(i18n.Resource))]
        public bool ShowNote { get; set; }
    

        [Display(Name = "Identifier", ResourceType = typeof(i18n.Resource))]
        public CategoryAdditionalFieldVM Identifier { get; set; }

        [Display(Name = "Competence", ResourceType = typeof(i18n.Resource))]
        public CategoryAdditionalFieldVM Competence { get; set; }

        [Display(Name = "Validity", ResourceType = typeof(i18n.Resource))]
        public CategoryAdditionalFieldVM Validity { get; set; }

        [Display(Name = "DocumentView", ResourceType = typeof(i18n.Resource))]
        public CategoryAdditionalFieldVM DocumentView { get; set; }

        [Display(Name = "Note", ResourceType = typeof(i18n.Resource))]
        public CategoryAdditionalFieldVM Note { get; set; }
    }
}