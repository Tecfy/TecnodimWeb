﻿using System.ComponentModel.DataAnnotations;

namespace Model.VM
{
    public class UserUnityVM
    {
        [Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
        public int UserUnityId { get; set; }

        [Display(Name = "Unity", ResourceType = typeof(i18n.Resource))]
        public int UnityId { get; set; }

        [Display(Name = "User", ResourceType = typeof(i18n.Resource))]
        public int UserId { get; set; }
    }
}
