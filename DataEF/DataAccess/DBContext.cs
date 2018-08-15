﻿

// This file was automatically generated.
// Do not make changes directly to this file - edit the template instead.
// 
// The following connection settings were used to generate this file
// 
//     Configuration file:     "DataEF\App.config"
//     Connection String Name: "Default"
//     Connection String:      "Server=DEVPLACE002;Database=Tecnodim;User Id=Tecnodim; password=**zapped**;"

// ReSharper disable RedundantUsingDirective
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable PartialMethodWithSinglePart

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
//using DatabaseGeneratedOption = System.ComponentModel.DataAnnotations.DatabaseGeneratedOption;

using System.ComponentModel.DataAnnotations;

namespace DataEF.DataAccess
{
    // ************************************************************************
    // Unit of work
    public interface IDBContext : IDisposable
    {
        IDbSet<AdditionalFields> AdditionalFields { get; set; } // AdditionalFields
        IDbSet<ApiUsers> ApiUsers { get; set; } // ApiUsers
        IDbSet<AspNetRoles> AspNetRoles { get; set; } // AspNetRoles
        IDbSet<AspNetUserClaims> AspNetUserClaims { get; set; } // AspNetUserClaims
        IDbSet<AspNetUserLogins> AspNetUserLogins { get; set; } // AspNetUserLogins
        IDbSet<AspNetUserRoles> AspNetUserRoles { get; set; } // AspNetUserRoles
        IDbSet<AspNetUsers> AspNetUsers { get; set; } // AspNetUsers
        IDbSet<Categories> Categories { get; set; } // Categories
        IDbSet<CategoryAdditionalFields> CategoryAdditionalFields { get; set; } // CategoryAdditionalFields
        IDbSet<DeletedPages> DeletedPages { get; set; } // DeletedPages
        IDbSet<Documents> Documents { get; set; } // Documents
        IDbSet<DocumentStatus> DocumentStatus { get; set; } // DocumentStatus
        IDbSet<RegisterEvents> RegisterEvents { get; set; } // RegisterEvents
        IDbSet<SliceCategoryAdditionalFields> SliceCategoryAdditionalFields { get; set; } // SliceCategoryAdditionalFields
        IDbSet<SlicePages> SlicePages { get; set; } // SlicePages
        IDbSet<Slices> Slices { get; set; } // Slices

        int SaveChanges();
    }

    // ************************************************************************
    // Database context
    public partial class DBContext : DbContext, IDBContext
    {
        public IDbSet<AdditionalFields> AdditionalFields { get; set; } // AdditionalFields
        public IDbSet<ApiUsers> ApiUsers { get; set; } // ApiUsers
        public IDbSet<AspNetRoles> AspNetRoles { get; set; } // AspNetRoles
        public IDbSet<AspNetUserClaims> AspNetUserClaims { get; set; } // AspNetUserClaims
        public IDbSet<AspNetUserLogins> AspNetUserLogins { get; set; } // AspNetUserLogins
        public IDbSet<AspNetUserRoles> AspNetUserRoles { get; set; } // AspNetUserRoles
        public IDbSet<AspNetUsers> AspNetUsers { get; set; } // AspNetUsers
        public IDbSet<Categories> Categories { get; set; } // Categories
        public IDbSet<CategoryAdditionalFields> CategoryAdditionalFields { get; set; } // CategoryAdditionalFields
        public IDbSet<DeletedPages> DeletedPages { get; set; } // DeletedPages
        public IDbSet<Documents> Documents { get; set; } // Documents
        public IDbSet<DocumentStatus> DocumentStatus { get; set; } // DocumentStatus
        public IDbSet<RegisterEvents> RegisterEvents { get; set; } // RegisterEvents
        public IDbSet<SliceCategoryAdditionalFields> SliceCategoryAdditionalFields { get; set; } // SliceCategoryAdditionalFields
        public IDbSet<SlicePages> SlicePages { get; set; } // SlicePages
        public IDbSet<Slices> Slices { get; set; } // Slices

        static DBContext()
        {
            Database.SetInitializer<DBContext>(null);
        }

        public DBContext()
            : base("Name=Default")
        {
        InitializePartial();
        }

        public DBContext(string connectionString) : base(connectionString)
        {
        InitializePartial();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new AdditionalFieldsConfiguration());
            modelBuilder.Configurations.Add(new ApiUsersConfiguration());
            modelBuilder.Configurations.Add(new AspNetRolesConfiguration());
            modelBuilder.Configurations.Add(new AspNetUserClaimsConfiguration());
            modelBuilder.Configurations.Add(new AspNetUserLoginsConfiguration());
            modelBuilder.Configurations.Add(new AspNetUserRolesConfiguration());
            modelBuilder.Configurations.Add(new AspNetUsersConfiguration());
            modelBuilder.Configurations.Add(new CategoriesConfiguration());
            modelBuilder.Configurations.Add(new CategoryAdditionalFieldsConfiguration());
            modelBuilder.Configurations.Add(new DeletedPagesConfiguration());
            modelBuilder.Configurations.Add(new DocumentsConfiguration());
            modelBuilder.Configurations.Add(new DocumentStatusConfiguration());
            modelBuilder.Configurations.Add(new RegisterEventsConfiguration());
            modelBuilder.Configurations.Add(new SliceCategoryAdditionalFieldsConfiguration());
            modelBuilder.Configurations.Add(new SlicePagesConfiguration());
            modelBuilder.Configurations.Add(new SlicesConfiguration());
        OnModelCreatingPartial(modelBuilder);
        }

        partial void InitializePartial();
        partial void OnModelCreatingPartial(DbModelBuilder modelBuilder);
    }

    // ************************************************************************
    // POCO classes

	public partial class AdditionalFieldsMetadataType
    {
		/* 
		///Copy this class to an external file

		[Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
		public int AdditionalFieldId { get; set; } // AdditionalFieldId (Primary key)

		[Display(Name = "Active", ResourceType = typeof(i18n.Resource))]
		public bool Active { get; set; } // Active

		[Display(Name = "CreatedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime CreatedDate { get; set; } // CreatedDate

		[Display(Name = "EditedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime? EditedDate { get; set; } // EditedDate

		[Display(Name = "DeletedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime? DeletedDate { get; set; } // DeletedDate

		[StringLength(255, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
		[Display(Name = "Name", ResourceType = typeof(i18n.Resource))]
		public string Name { get; set; } // Name

		[StringLength(50, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
		[Display(Name = "Type", ResourceType = typeof(i18n.Resource))]
		public string Type { get; set; } // Type

		*/
	}

    // AdditionalFields
	[MetadataType(typeof(AdditionalFieldsMetadataType))]
    public partial class AdditionalFields
    {

        [DataEF.Attributes.Template.IdentityField()]
        public int AdditionalFieldId { get; set; } // AdditionalFieldId (Primary key)

        public bool Active { get; set; } // Active

        [DataEF.Attributes.Template.ExcludeField()]
        public DateTime CreatedDate { get; set; } // CreatedDate

        [DataEF.Attributes.Template.ExcludeField()]
        public DateTime? EditedDate { get; set; } // EditedDate

        [DataEF.Attributes.Template.ExcludeField()]
        public DateTime? DeletedDate { get; set; } // DeletedDate

        public string Name { get; set; } // Name

        public string Type { get; set; } // Type

        // Reverse navigation
        public virtual ICollection<CategoryAdditionalFields> CategoryAdditionalFields { get; set; } // CategoryAdditionalFields.FK_CategoryAdditionalFields_AdditionalFields;

        public AdditionalFields()
        {
            Active = true;
            CreatedDate = DateTime.Now;
            CategoryAdditionalFields = new List<CategoryAdditionalFields>();
            InitializePartial();
        }
        partial void InitializePartial();
    }

	public partial class ApiUsersMetadataType
    {
		/* 
		///Copy this class to an external file

		[Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
		public int ApiUserId { get; set; } // ApiUserId (Primary key)

		[Display(Name = "Active", ResourceType = typeof(i18n.Resource))]
		public bool Active { get; set; } // Active

		[Display(Name = "CreatedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime CreatedDate { get; set; } // CreatedDate

		[Display(Name = "EditedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime? EditedDate { get; set; } // EditedDate

		[Display(Name = "DeletedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime? DeletedDate { get; set; } // DeletedDate

		[StringLength(250, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
		[Display(Name = "User", ResourceType = typeof(i18n.Resource))]
		public string User { get; set; } // User

		[Display(Name = "Hash", ResourceType = typeof(i18n.Resource))]
		public Guid Hash { get; set; } // Hash

		[Display(Name = "LastLogin", ResourceType = typeof(i18n.Resource))]
		public DateTime? LastLogin { get; set; } // LastLogin

		*/
	}

    // ApiUsers
	[MetadataType(typeof(ApiUsersMetadataType))]
    public partial class ApiUsers
    {

        [DataEF.Attributes.Template.IdentityField()]
        public int ApiUserId { get; set; } // ApiUserId (Primary key)

        public bool Active { get; set; } // Active

        [DataEF.Attributes.Template.ExcludeField()]
        public DateTime CreatedDate { get; set; } // CreatedDate

        [DataEF.Attributes.Template.ExcludeField()]
        public DateTime? EditedDate { get; set; } // EditedDate

        [DataEF.Attributes.Template.ExcludeField()]
        public DateTime? DeletedDate { get; set; } // DeletedDate

        public string User { get; set; } // User

        public Guid Hash { get; set; } // Hash

        public DateTime? LastLogin { get; set; } // LastLogin

        public ApiUsers()
        {
            Active = true;
            CreatedDate = DateTime.Now;
            Hash = Guid.NewGuid();
            InitializePartial();
        }
        partial void InitializePartial();
    }

	public partial class AspNetRolesMetadataType
    {
		/* 
		///Copy this class to an external file

		[StringLength(128, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
		[Display(Name = "Id", ResourceType = typeof(i18n.Resource))]
		public string Id { get; set; } // Id (Primary key)

		[StringLength(256, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
		[Display(Name = "Name", ResourceType = typeof(i18n.Resource))]
		public string Name { get; set; } // Name

		*/
	}

    // AspNetRoles
	[MetadataType(typeof(AspNetRolesMetadataType))]
    public partial class AspNetRoles
    {

        public string Id { get; set; } // Id (Primary key)

        public string Name { get; set; } // Name

        // Reverse navigation
        public virtual ICollection<AspNetUserRoles> AspNetUserRoles { get; set; } // AspNetUserRoles.FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId;

        public AspNetRoles()
        {
            AspNetUserRoles = new List<AspNetUserRoles>();
            InitializePartial();
        }
        partial void InitializePartial();
    }

	public partial class AspNetUserClaimsMetadataType
    {
		/* 
		///Copy this class to an external file

		[Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
		public int Id { get; set; } // Id (Primary key)

		[StringLength(128, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
		[Display(Name = "User", ResourceType = typeof(i18n.Resource))]
		public string UserId { get; set; } // UserId

		[Display(Name = "ClaimType", ResourceType = typeof(i18n.Resource))]
		public string ClaimType { get; set; } // ClaimType

		[Display(Name = "ClaimValue", ResourceType = typeof(i18n.Resource))]
		public string ClaimValue { get; set; } // ClaimValue

		*/
	}

    // AspNetUserClaims
	[MetadataType(typeof(AspNetUserClaimsMetadataType))]
    public partial class AspNetUserClaims
    {

        [DataEF.Attributes.Template.IdentityField()]
        public int Id { get; set; } // Id (Primary key)

        public string UserId { get; set; } // UserId

        public string ClaimType { get; set; } // ClaimType

        public string ClaimValue { get; set; } // ClaimValue

        // Foreign keys
        public virtual AspNetUsers AspNetUsers { get; set; } //  UserId - FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId
    }

	public partial class AspNetUserLoginsMetadataType
    {
		/* 
		///Copy this class to an external file

		[StringLength(128, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
		[Display(Name = "LoginProvider", ResourceType = typeof(i18n.Resource))]
		public string LoginProvider { get; set; } // LoginProvider (Primary key)

		[StringLength(128, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
		[Display(Name = "ProviderKey", ResourceType = typeof(i18n.Resource))]
		public string ProviderKey { get; set; } // ProviderKey (Primary key)

		[StringLength(128, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
		[Display(Name = "User", ResourceType = typeof(i18n.Resource))]
		public string UserId { get; set; } // UserId (Primary key)

		*/
	}

    // AspNetUserLogins
	[MetadataType(typeof(AspNetUserLoginsMetadataType))]
    public partial class AspNetUserLogins
    {

        public string LoginProvider { get; set; } // LoginProvider (Primary key)

        public string ProviderKey { get; set; } // ProviderKey (Primary key)

        public string UserId { get; set; } // UserId (Primary key)

        // Foreign keys
        public virtual AspNetUsers AspNetUsers { get; set; } //  UserId - FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId
    }

	public partial class AspNetUserRolesMetadataType
    {
		/* 
		///Copy this class to an external file

		[StringLength(128, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
		[Display(Name = "User", ResourceType = typeof(i18n.Resource))]
		public string UserId { get; set; } // UserId (Primary key)

		[StringLength(128, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
		[Display(Name = "Role", ResourceType = typeof(i18n.Resource))]
		public string RoleId { get; set; } // RoleId (Primary key)

		*/
	}

    // AspNetUserRoles
	[MetadataType(typeof(AspNetUserRolesMetadataType))]
    public partial class AspNetUserRoles
    {

        public string UserId { get; set; } // UserId (Primary key)

        public string RoleId { get; set; } // RoleId (Primary key)

        // Foreign keys
        public virtual AspNetUsers AspNetUsers { get; set; } //  UserId - FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId
        public virtual AspNetRoles AspNetRoles { get; set; } //  RoleId - FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId
    }

	public partial class AspNetUsersMetadataType
    {
		/* 
		///Copy this class to an external file

		[StringLength(128, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
		[Display(Name = "Id", ResourceType = typeof(i18n.Resource))]
		public string Id { get; set; } // Id (Primary key)

		[StringLength(256, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
		[Display(Name = "Email", ResourceType = typeof(i18n.Resource))]
		public string Email { get; set; } // Email

		[Display(Name = "EmailConfirmed", ResourceType = typeof(i18n.Resource))]
		public bool EmailConfirmed { get; set; } // EmailConfirmed

		[Display(Name = "PasswordHash", ResourceType = typeof(i18n.Resource))]
		public string PasswordHash { get; set; } // PasswordHash

		[Display(Name = "SecurityStamp", ResourceType = typeof(i18n.Resource))]
		public string SecurityStamp { get; set; } // SecurityStamp

		[Display(Name = "PhoneNumber", ResourceType = typeof(i18n.Resource))]
		public string PhoneNumber { get; set; } // PhoneNumber

		[Display(Name = "PhoneNumberConfirmed", ResourceType = typeof(i18n.Resource))]
		public bool PhoneNumberConfirmed { get; set; } // PhoneNumberConfirmed

		[Display(Name = "TwoFactorEnabled", ResourceType = typeof(i18n.Resource))]
		public bool TwoFactorEnabled { get; set; } // TwoFactorEnabled

		[Display(Name = "LockoutEndDateUtc", ResourceType = typeof(i18n.Resource))]
		public DateTime? LockoutEndDateUtc { get; set; } // LockoutEndDateUtc

		[Display(Name = "LockoutEnabled", ResourceType = typeof(i18n.Resource))]
		public bool LockoutEnabled { get; set; } // LockoutEnabled

		[Display(Name = "AccessFailedCount", ResourceType = typeof(i18n.Resource))]
		public int AccessFailedCount { get; set; } // AccessFailedCount

		[StringLength(256, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
		[Display(Name = "UserName", ResourceType = typeof(i18n.Resource))]
		public string UserName { get; set; } // UserName

		*/
	}

    // AspNetUsers
	[MetadataType(typeof(AspNetUsersMetadataType))]
    public partial class AspNetUsers
    {

        public string Id { get; set; } // Id (Primary key)

        public string Email { get; set; } // Email

        public bool EmailConfirmed { get; set; } // EmailConfirmed

        public string PasswordHash { get; set; } // PasswordHash

        public string SecurityStamp { get; set; } // SecurityStamp

        public string PhoneNumber { get; set; } // PhoneNumber

        public bool PhoneNumberConfirmed { get; set; } // PhoneNumberConfirmed

        public bool TwoFactorEnabled { get; set; } // TwoFactorEnabled

        public DateTime? LockoutEndDateUtc { get; set; } // LockoutEndDateUtc

        public bool LockoutEnabled { get; set; } // LockoutEnabled

        public int AccessFailedCount { get; set; } // AccessFailedCount

        public string UserName { get; set; } // UserName

        // Reverse navigation
        public virtual ICollection<AspNetUserClaims> AspNetUserClaims { get; set; } // AspNetUserClaims.FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId;
        public virtual ICollection<AspNetUserLogins> AspNetUserLogins { get; set; } // AspNetUserLogins.FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId;
        public virtual ICollection<AspNetUserRoles> AspNetUserRoles { get; set; } // AspNetUserRoles.FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId;

        public AspNetUsers()
        {
            AspNetUserClaims = new List<AspNetUserClaims>();
            AspNetUserLogins = new List<AspNetUserLogins>();
            AspNetUserRoles = new List<AspNetUserRoles>();
            InitializePartial();
        }
        partial void InitializePartial();
    }

	public partial class CategoriesMetadataType
    {
		/* 
		///Copy this class to an external file

		[Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
		public int CategoryId { get; set; } // CategoryId (Primary key)

		[Display(Name = "Active", ResourceType = typeof(i18n.Resource))]
		public bool Active { get; set; } // Active

		[Display(Name = "CreatedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime CreatedDate { get; set; } // CreatedDate

		[Display(Name = "EditedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime? EditedDate { get; set; } // EditedDate

		[Display(Name = "DeletedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime? DeletedDate { get; set; } // DeletedDate

		[Display(Name = "Parent", ResourceType = typeof(i18n.Resource))]
		public int? ParentId { get; set; } // ParentId

		[StringLength(50, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
		[Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
		public string Code { get; set; } // Code

		[StringLength(255, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
		[Display(Name = "Name", ResourceType = typeof(i18n.Resource))]
		public string Name { get; set; } // Name

		*/
	}

    // Categories
	[MetadataType(typeof(CategoriesMetadataType))]
    public partial class Categories
    {

        [DataEF.Attributes.Template.IdentityField()]
        public int CategoryId { get; set; } // CategoryId (Primary key)

        public bool Active { get; set; } // Active

        [DataEF.Attributes.Template.ExcludeField()]
        public DateTime CreatedDate { get; set; } // CreatedDate

        [DataEF.Attributes.Template.ExcludeField()]
        public DateTime? EditedDate { get; set; } // EditedDate

        [DataEF.Attributes.Template.ExcludeField()]
        public DateTime? DeletedDate { get; set; } // DeletedDate

        public int? ParentId { get; set; } // ParentId

        public string Code { get; set; } // Code

        public string Name { get; set; } // Name

        // Reverse navigation
        public virtual ICollection<Categories> Categories2 { get; set; } // Categories.FK_Categories_Parent;
        public virtual ICollection<CategoryAdditionalFields> CategoryAdditionalFields { get; set; } // CategoryAdditionalFields.FK_CategoryAdditionalFields_Categories;
        public virtual ICollection<Slices> Slices { get; set; } // Slices.FK_Slices_Categories;

        // Foreign keys
        public virtual Categories Categories1 { get; set; } //  ParentId - FK_Categories_Parent

        public Categories()
        {
            Active = true;
            CreatedDate = DateTime.Now;
            Categories2 = new List<Categories>();
            CategoryAdditionalFields = new List<CategoryAdditionalFields>();
            Slices = new List<Slices>();
            InitializePartial();
        }
        partial void InitializePartial();
    }

	public partial class CategoryAdditionalFieldsMetadataType
    {
		/* 
		///Copy this class to an external file

		[Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
		public int CategoryAdditionalFieldId { get; set; } // CategoryAdditionalFieldId (Primary key)

		[Display(Name = "Category", ResourceType = typeof(i18n.Resource))]
		public int CategoryId { get; set; } // CategoryId

		[Display(Name = "AdditionalField", ResourceType = typeof(i18n.Resource))]
		public int AdditionalFieldId { get; set; } // AdditionalFieldId

		[Display(Name = "Single", ResourceType = typeof(i18n.Resource))]
		public bool Single { get; set; } // Single

		[Display(Name = "Required", ResourceType = typeof(i18n.Resource))]
		public bool Required { get; set; } // Required

		[Display(Name = "Confidential", ResourceType = typeof(i18n.Resource))]
		public bool Confidential { get; set; } // Confidential

		*/
	}

    // CategoryAdditionalFields
	[MetadataType(typeof(CategoryAdditionalFieldsMetadataType))]
    public partial class CategoryAdditionalFields
    {

        [DataEF.Attributes.Template.IdentityField()]
        public int CategoryAdditionalFieldId { get; set; } // CategoryAdditionalFieldId (Primary key)

        public int CategoryId { get; set; } // CategoryId

        public int AdditionalFieldId { get; set; } // AdditionalFieldId

        public bool Single { get; set; } // Single

        public bool Required { get; set; } // Required

        public bool Confidential { get; set; } // Confidential

        // Reverse navigation
        public virtual ICollection<SliceCategoryAdditionalFields> SliceCategoryAdditionalFields { get; set; } // SliceCategoryAdditionalFields.FK_SliceCategoryAdditionalFields_CategoryAdditionalFields;

        // Foreign keys
        public virtual Categories Categories { get; set; } //  CategoryId - FK_CategoryAdditionalFields_Categories
        public virtual AdditionalFields AdditionalFields { get; set; } //  AdditionalFieldId - FK_CategoryAdditionalFields_AdditionalFields

        public CategoryAdditionalFields()
        {
            Single = false;
            Required = false;
            Confidential = false;
            SliceCategoryAdditionalFields = new List<SliceCategoryAdditionalFields>();
            InitializePartial();
        }
        partial void InitializePartial();
    }

	public partial class DeletedPagesMetadataType
    {
		/* 
		///Copy this class to an external file

		[Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
		public int DeletedPageId { get; set; } // DeletedPageId (Primary key)

		[Display(Name = "Active", ResourceType = typeof(i18n.Resource))]
		public bool Active { get; set; } // Active

		[Display(Name = "CreatedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime CreatedDate { get; set; } // CreatedDate

		[Display(Name = "EditedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime? EditedDate { get; set; } // EditedDate

		[Display(Name = "DeletedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime? DeletedDate { get; set; } // DeletedDate

		[Display(Name = "Document", ResourceType = typeof(i18n.Resource))]
		public int DocumentId { get; set; } // DocumentId

		[Display(Name = "Page", ResourceType = typeof(i18n.Resource))]
		public int Page { get; set; } // Page

		*/
	}

    // DeletedPages
	[MetadataType(typeof(DeletedPagesMetadataType))]
    public partial class DeletedPages
    {

        [DataEF.Attributes.Template.IdentityField()]
        public int DeletedPageId { get; set; } // DeletedPageId (Primary key)

        public bool Active { get; set; } // Active

        [DataEF.Attributes.Template.ExcludeField()]
        public DateTime CreatedDate { get; set; } // CreatedDate

        [DataEF.Attributes.Template.ExcludeField()]
        public DateTime? EditedDate { get; set; } // EditedDate

        [DataEF.Attributes.Template.ExcludeField()]
        public DateTime? DeletedDate { get; set; } // DeletedDate

        public int DocumentId { get; set; } // DocumentId

        public int Page { get; set; } // Page

        // Foreign keys
        public virtual Documents Documents { get; set; } //  DocumentId - FK_DeletedPages_Documents

        public DeletedPages()
        {
            Active = true;
            CreatedDate = DateTime.Now;
            InitializePartial();
        }
        partial void InitializePartial();
    }

	public partial class DocumentsMetadataType
    {
		/* 
		///Copy this class to an external file

		[Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
		public int DocumentId { get; set; } // DocumentId (Primary key)

		[Display(Name = "Active", ResourceType = typeof(i18n.Resource))]
		public bool Active { get; set; } // Active

		[Display(Name = "CreatedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime CreatedDate { get; set; } // CreatedDate

		[Display(Name = "EditedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime? EditedDate { get; set; } // EditedDate

		[Display(Name = "DeletedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime? DeletedDate { get; set; } // DeletedDate

		[Display(Name = "ExternalId", ResourceType = typeof(i18n.Resource))]
		public int ExternalId { get; set; } // ExternalId

		[Display(Name = "DocumentStatus", ResourceType = typeof(i18n.Resource))]
		public int DocumentStatusId { get; set; } // DocumentStatusId

		*/
	}

    // Documents
	[MetadataType(typeof(DocumentsMetadataType))]
    public partial class Documents
    {

        [DataEF.Attributes.Template.IdentityField()]
        public int DocumentId { get; set; } // DocumentId (Primary key)

        public bool Active { get; set; } // Active

        [DataEF.Attributes.Template.ExcludeField()]
        public DateTime CreatedDate { get; set; } // CreatedDate

        [DataEF.Attributes.Template.ExcludeField()]
        public DateTime? EditedDate { get; set; } // EditedDate

        [DataEF.Attributes.Template.ExcludeField()]
        public DateTime? DeletedDate { get; set; } // DeletedDate

        public int ExternalId { get; set; } // ExternalId

        public int DocumentStatusId { get; set; } // DocumentStatusId

        // Reverse navigation
        public virtual ICollection<DeletedPages> DeletedPages { get; set; } // DeletedPages.FK_DeletedPages_Documents;
        public virtual ICollection<Slices> Slices { get; set; } // Slices.FK_Slices_Documents;

        // Foreign keys
        public virtual DocumentStatus DocumentStatus { get; set; } //  DocumentStatusId - FK_Documents_DocumentStatus

        public Documents()
        {
            Active = true;
            CreatedDate = DateTime.Now;
            DeletedPages = new List<DeletedPages>();
            Slices = new List<Slices>();
            InitializePartial();
        }
        partial void InitializePartial();
    }

	public partial class DocumentStatusMetadataType
    {
		/* 
		///Copy this class to an external file

		[Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
		public int DocumentStatusId { get; set; } // DocumentStatusId (Primary key)

		[Display(Name = "Active", ResourceType = typeof(i18n.Resource))]
		public bool Active { get; set; } // Active

		[Display(Name = "CreatedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime CreatedDate { get; set; } // CreatedDate

		[Display(Name = "EditedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime? EditedDate { get; set; } // EditedDate

		[Display(Name = "DeletedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime? DeletedDate { get; set; } // DeletedDate

		[StringLength(255, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
		[Display(Name = "Name", ResourceType = typeof(i18n.Resource))]
		public string Name { get; set; } // Name

		*/
	}

    // DocumentStatus
	[MetadataType(typeof(DocumentStatusMetadataType))]
    public partial class DocumentStatus
    {

        [DataEF.Attributes.Template.IdentityField()]
        public int DocumentStatusId { get; set; } // DocumentStatusId (Primary key)

        public bool Active { get; set; } // Active

        [DataEF.Attributes.Template.ExcludeField()]
        public DateTime CreatedDate { get; set; } // CreatedDate

        [DataEF.Attributes.Template.ExcludeField()]
        public DateTime? EditedDate { get; set; } // EditedDate

        [DataEF.Attributes.Template.ExcludeField()]
        public DateTime? DeletedDate { get; set; } // DeletedDate

        public string Name { get; set; } // Name

        // Reverse navigation
        public virtual ICollection<Documents> Documents { get; set; } // Documents.FK_Documents_DocumentStatus;

        public DocumentStatus()
        {
            Active = true;
            CreatedDate = DateTime.Now;
            Documents = new List<Documents>();
            InitializePartial();
        }
        partial void InitializePartial();
    }

	public partial class RegisterEventsMetadataType
    {
		/* 
		///Copy this class to an external file

		[Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
		public int RegisterEventId { get; set; } // RegisterEventId (Primary key)

		[Display(Name = "Active", ResourceType = typeof(i18n.Resource))]
		public bool Active { get; set; } // Active

		[Display(Name = "CreatedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime CreatedDate { get; set; } // CreatedDate

		[Display(Name = "EditedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime? EditedDate { get; set; } // EditedDate

		[Display(Name = "DeletedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime? DeletedDate { get; set; } // DeletedDate

		[Display(Name = "UserId", ResourceType = typeof(i18n.Resource))]
		public Guid UserId { get; set; } // UserId

		[Display(Name = "Identifier", ResourceType = typeof(i18n.Resource))]
		public Guid Identifier { get; set; } // Identifier

		[StringLength(100, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
		[Display(Name = "Type", ResourceType = typeof(i18n.Resource))]
		public string Type { get; set; } // Type

		[Display(Name = "Source", ResourceType = typeof(i18n.Resource))]
		public string Source { get; set; } // Source

		[Display(Name = "Text", ResourceType = typeof(i18n.Resource))]
		public string Text { get; set; } // Text

		*/
	}

    // RegisterEvents
	[MetadataType(typeof(RegisterEventsMetadataType))]
    public partial class RegisterEvents
    {

        [DataEF.Attributes.Template.IdentityField()]
        public int RegisterEventId { get; set; } // RegisterEventId (Primary key)

        public bool Active { get; set; } // Active

        [DataEF.Attributes.Template.ExcludeField()]
        public DateTime CreatedDate { get; set; } // CreatedDate

        [DataEF.Attributes.Template.ExcludeField()]
        public DateTime? EditedDate { get; set; } // EditedDate

        [DataEF.Attributes.Template.ExcludeField()]
        public DateTime? DeletedDate { get; set; } // DeletedDate

        public Guid UserId { get; set; } // UserId

        public Guid Identifier { get; set; } // Identifier

        public string Type { get; set; } // Type

        public string Source { get; set; } // Source

        public string Text { get; set; } // Text

        public RegisterEvents()
        {
            Active = true;
            CreatedDate = DateTime.Now;
            Identifier = Guid.NewGuid();
            InitializePartial();
        }
        partial void InitializePartial();
    }

	public partial class SliceCategoryAdditionalFieldsMetadataType
    {
		/* 
		///Copy this class to an external file

		[Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
		public int SliceCategoryAdditionalFieldId { get; set; } // SliceCategoryAdditionalFieldId (Primary key)

		[Display(Name = "Active", ResourceType = typeof(i18n.Resource))]
		public bool Active { get; set; } // Active

		[Display(Name = "CreatedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime CreatedDate { get; set; } // CreatedDate

		[Display(Name = "EditedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime? EditedDate { get; set; } // EditedDate

		[Display(Name = "DeletedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime? DeletedDate { get; set; } // DeletedDate

		[Display(Name = "Slice", ResourceType = typeof(i18n.Resource))]
		public int SliceId { get; set; } // SliceId

		[Display(Name = "CategoryAdditionalField", ResourceType = typeof(i18n.Resource))]
		public int CategoryAdditionalFieldId { get; set; } // CategoryAdditionalFieldId

		[StringLength(255, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
		[Display(Name = "Value", ResourceType = typeof(i18n.Resource))]
		public string Value { get; set; } // Value

		*/
	}

    // SliceCategoryAdditionalFields
	[MetadataType(typeof(SliceCategoryAdditionalFieldsMetadataType))]
    public partial class SliceCategoryAdditionalFields
    {

        [DataEF.Attributes.Template.IdentityField()]
        public int SliceCategoryAdditionalFieldId { get; set; } // SliceCategoryAdditionalFieldId (Primary key)

        public bool Active { get; set; } // Active

        [DataEF.Attributes.Template.ExcludeField()]
        public DateTime CreatedDate { get; set; } // CreatedDate

        [DataEF.Attributes.Template.ExcludeField()]
        public DateTime? EditedDate { get; set; } // EditedDate

        [DataEF.Attributes.Template.ExcludeField()]
        public DateTime? DeletedDate { get; set; } // DeletedDate

        public int SliceId { get; set; } // SliceId

        public int CategoryAdditionalFieldId { get; set; } // CategoryAdditionalFieldId

        public string Value { get; set; } // Value

        // Foreign keys
        public virtual Slices Slices { get; set; } //  SliceId - FK_SliceCategoryAdditionalFields_Slices
        public virtual CategoryAdditionalFields CategoryAdditionalFields { get; set; } //  CategoryAdditionalFieldId - FK_SliceCategoryAdditionalFields_CategoryAdditionalFields

        public SliceCategoryAdditionalFields()
        {
            Active = true;
            CreatedDate = DateTime.Now;
            InitializePartial();
        }
        partial void InitializePartial();
    }

	public partial class SlicePagesMetadataType
    {
		/* 
		///Copy this class to an external file

		[Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
		public int SlicePageId { get; set; } // SlicePageId (Primary key)

		[Display(Name = "Active", ResourceType = typeof(i18n.Resource))]
		public bool Active { get; set; } // Active

		[Display(Name = "CreatedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime CreatedDate { get; set; } // CreatedDate

		[Display(Name = "EditedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime? EditedDate { get; set; } // EditedDate

		[Display(Name = "DeletedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime? DeletedDate { get; set; } // DeletedDate

		[Display(Name = "Slice", ResourceType = typeof(i18n.Resource))]
		public int SliceId { get; set; } // SliceId

		[Display(Name = "Page", ResourceType = typeof(i18n.Resource))]
		public int Page { get; set; } // Page

		[Display(Name = "Rotate", ResourceType = typeof(i18n.Resource))]
		public int? Rotate { get; set; } // Rotate

		*/
	}

    // SlicePages
	[MetadataType(typeof(SlicePagesMetadataType))]
    public partial class SlicePages
    {

        [DataEF.Attributes.Template.IdentityField()]
        public int SlicePageId { get; set; } // SlicePageId (Primary key)

        public bool Active { get; set; } // Active

        [DataEF.Attributes.Template.ExcludeField()]
        public DateTime CreatedDate { get; set; } // CreatedDate

        [DataEF.Attributes.Template.ExcludeField()]
        public DateTime? EditedDate { get; set; } // EditedDate

        [DataEF.Attributes.Template.ExcludeField()]
        public DateTime? DeletedDate { get; set; } // DeletedDate

        public int SliceId { get; set; } // SliceId

        public int Page { get; set; } // Page

        public int? Rotate { get; set; } // Rotate

        // Foreign keys
        public virtual Slices Slices { get; set; } //  SliceId - FK_SlicePages_Slices

        public SlicePages()
        {
            Active = true;
            CreatedDate = DateTime.Now;
            InitializePartial();
        }
        partial void InitializePartial();
    }

	public partial class SlicesMetadataType
    {
		/* 
		///Copy this class to an external file

		[Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
		public int SliceId { get; set; } // SliceId (Primary key)

		[Display(Name = "Active", ResourceType = typeof(i18n.Resource))]
		public bool Active { get; set; } // Active

		[Display(Name = "CreatedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime CreatedDate { get; set; } // CreatedDate

		[Display(Name = "EditedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime? EditedDate { get; set; } // EditedDate

		[Display(Name = "DeletedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime? DeletedDate { get; set; } // DeletedDate

		[Display(Name = "Document", ResourceType = typeof(i18n.Resource))]
		public int DocumentId { get; set; } // DocumentId

		[Display(Name = "Category", ResourceType = typeof(i18n.Resource))]
		public int? CategoryId { get; set; } // CategoryId

		[StringLength(255, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
		[Display(Name = "Name", ResourceType = typeof(i18n.Resource))]
		public string Name { get; set; } // Name

		[Display(Name = "Sent", ResourceType = typeof(i18n.Resource))]
		public bool Sent { get; set; } // Sent

		*/
	}

    // Slices
	[MetadataType(typeof(SlicesMetadataType))]
    public partial class Slices
    {

        [DataEF.Attributes.Template.IdentityField()]
        public int SliceId { get; set; } // SliceId (Primary key)

        public bool Active { get; set; } // Active

        [DataEF.Attributes.Template.ExcludeField()]
        public DateTime CreatedDate { get; set; } // CreatedDate

        [DataEF.Attributes.Template.ExcludeField()]
        public DateTime? EditedDate { get; set; } // EditedDate

        [DataEF.Attributes.Template.ExcludeField()]
        public DateTime? DeletedDate { get; set; } // DeletedDate

        public int DocumentId { get; set; } // DocumentId

        public int? CategoryId { get; set; } // CategoryId

        public string Name { get; set; } // Name

        public bool Sent { get; set; } // Sent

        // Reverse navigation
        public virtual ICollection<SliceCategoryAdditionalFields> SliceCategoryAdditionalFields { get; set; } // SliceCategoryAdditionalFields.FK_SliceCategoryAdditionalFields_Slices;
        public virtual ICollection<SlicePages> SlicePages { get; set; } // SlicePages.FK_SlicePages_Slices;

        // Foreign keys
        public virtual Documents Documents { get; set; } //  DocumentId - FK_Slices_Documents
        public virtual Categories Categories { get; set; } //  CategoryId - FK_Slices_Categories

        public Slices()
        {
            Active = true;
            CreatedDate = DateTime.Now;
            Sent = false;
            SliceCategoryAdditionalFields = new List<SliceCategoryAdditionalFields>();
            SlicePages = new List<SlicePages>();
            InitializePartial();
        }
        partial void InitializePartial();
    }


    // ************************************************************************
    // POCO Configuration

    // AdditionalFields
    internal partial class AdditionalFieldsConfiguration : EntityTypeConfiguration<AdditionalFields>
    {
        public AdditionalFieldsConfiguration()
        {
            ToTable("dbo.AdditionalFields");
            HasKey(x => x.AdditionalFieldId);

            Property(x => x.AdditionalFieldId).HasColumnName("AdditionalFieldId").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Active).HasColumnName("Active").IsRequired();
            Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            Property(x => x.EditedDate).HasColumnName("EditedDate").IsOptional();
            Property(x => x.DeletedDate).HasColumnName("DeletedDate").IsOptional();
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(255);
            Property(x => x.Type).HasColumnName("Type").IsRequired().HasMaxLength(50);
            InitializePartial();
        }
        partial void InitializePartial();
    }

    // ApiUsers
    internal partial class ApiUsersConfiguration : EntityTypeConfiguration<ApiUsers>
    {
        public ApiUsersConfiguration()
        {
            ToTable("dbo.ApiUsers");
            HasKey(x => x.ApiUserId);

            Property(x => x.ApiUserId).HasColumnName("ApiUserId").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Active).HasColumnName("Active").IsRequired();
            Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            Property(x => x.EditedDate).HasColumnName("EditedDate").IsOptional();
            Property(x => x.DeletedDate).HasColumnName("DeletedDate").IsOptional();
            Property(x => x.User).HasColumnName("User").IsRequired().HasMaxLength(250);
            Property(x => x.Hash).HasColumnName("Hash").IsRequired();
            Property(x => x.LastLogin).HasColumnName("LastLogin").IsOptional();
            InitializePartial();
        }
        partial void InitializePartial();
    }

    // AspNetRoles
    internal partial class AspNetRolesConfiguration : EntityTypeConfiguration<AspNetRoles>
    {
        public AspNetRolesConfiguration()
        {
            ToTable("dbo.AspNetRoles");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasMaxLength(128);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(256);
            InitializePartial();
        }
        partial void InitializePartial();
    }

    // AspNetUserClaims
    internal partial class AspNetUserClaimsConfiguration : EntityTypeConfiguration<AspNetUserClaims>
    {
        public AspNetUserClaimsConfiguration()
        {
            ToTable("dbo.AspNetUserClaims");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.UserId).HasColumnName("UserId").IsRequired().HasMaxLength(128);
            Property(x => x.ClaimType).HasColumnName("ClaimType").IsOptional();
            Property(x => x.ClaimValue).HasColumnName("ClaimValue").IsOptional();

            // Foreign keys
            HasRequired(a => a.AspNetUsers).WithMany(b => b.AspNetUserClaims).HasForeignKey(c => c.UserId); // FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId
            InitializePartial();
        }
        partial void InitializePartial();
    }

    // AspNetUserLogins
    internal partial class AspNetUserLoginsConfiguration : EntityTypeConfiguration<AspNetUserLogins>
    {
        public AspNetUserLoginsConfiguration()
        {
            ToTable("dbo.AspNetUserLogins");
            HasKey(x => new { x.LoginProvider, x.ProviderKey, x.UserId });

            Property(x => x.LoginProvider).HasColumnName("LoginProvider").IsRequired().HasMaxLength(128);
            Property(x => x.ProviderKey).HasColumnName("ProviderKey").IsRequired().HasMaxLength(128);
            Property(x => x.UserId).HasColumnName("UserId").IsRequired().HasMaxLength(128);

            // Foreign keys
            HasRequired(a => a.AspNetUsers).WithMany(b => b.AspNetUserLogins).HasForeignKey(c => c.UserId); // FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId
            InitializePartial();
        }
        partial void InitializePartial();
    }

    // AspNetUserRoles
    internal partial class AspNetUserRolesConfiguration : EntityTypeConfiguration<AspNetUserRoles>
    {
        public AspNetUserRolesConfiguration()
        {
            ToTable("dbo.AspNetUserRoles");
            HasKey(x => new { x.UserId, x.RoleId });

            Property(x => x.UserId).HasColumnName("UserId").IsRequired().HasMaxLength(128);
            Property(x => x.RoleId).HasColumnName("RoleId").IsRequired().HasMaxLength(128);

            // Foreign keys
            HasRequired(a => a.AspNetUsers).WithMany(b => b.AspNetUserRoles).HasForeignKey(c => c.UserId); // FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId
            HasRequired(a => a.AspNetRoles).WithMany(b => b.AspNetUserRoles).HasForeignKey(c => c.RoleId); // FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId
            InitializePartial();
        }
        partial void InitializePartial();
    }

    // AspNetUsers
    internal partial class AspNetUsersConfiguration : EntityTypeConfiguration<AspNetUsers>
    {
        public AspNetUsersConfiguration()
        {
            ToTable("dbo.AspNetUsers");
            HasKey(x => x.Id);

            Property(x => x.Id).HasColumnName("Id").IsRequired().HasMaxLength(128);
            Property(x => x.Email).HasColumnName("Email").IsOptional().HasMaxLength(256);
            Property(x => x.EmailConfirmed).HasColumnName("EmailConfirmed").IsRequired();
            Property(x => x.PasswordHash).HasColumnName("PasswordHash").IsOptional();
            Property(x => x.SecurityStamp).HasColumnName("SecurityStamp").IsOptional();
            Property(x => x.PhoneNumber).HasColumnName("PhoneNumber").IsOptional();
            Property(x => x.PhoneNumberConfirmed).HasColumnName("PhoneNumberConfirmed").IsRequired();
            Property(x => x.TwoFactorEnabled).HasColumnName("TwoFactorEnabled").IsRequired();
            Property(x => x.LockoutEndDateUtc).HasColumnName("LockoutEndDateUtc").IsOptional();
            Property(x => x.LockoutEnabled).HasColumnName("LockoutEnabled").IsRequired();
            Property(x => x.AccessFailedCount).HasColumnName("AccessFailedCount").IsRequired();
            Property(x => x.UserName).HasColumnName("UserName").IsRequired().HasMaxLength(256);
            InitializePartial();
        }
        partial void InitializePartial();
    }

    // Categories
    internal partial class CategoriesConfiguration : EntityTypeConfiguration<Categories>
    {
        public CategoriesConfiguration()
        {
            ToTable("dbo.Categories");
            HasKey(x => x.CategoryId);

            Property(x => x.CategoryId).HasColumnName("CategoryId").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Active).HasColumnName("Active").IsRequired();
            Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            Property(x => x.EditedDate).HasColumnName("EditedDate").IsOptional();
            Property(x => x.DeletedDate).HasColumnName("DeletedDate").IsOptional();
            Property(x => x.ParentId).HasColumnName("ParentId").IsOptional();
            Property(x => x.Code).HasColumnName("Code").IsRequired().HasMaxLength(50);
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(255);

            // Foreign keys
            HasOptional(a => a.Categories1).WithMany(b => b.Categories2).HasForeignKey(c => c.ParentId); // FK_Categories_Parent
            InitializePartial();
        }
        partial void InitializePartial();
    }

    // CategoryAdditionalFields
    internal partial class CategoryAdditionalFieldsConfiguration : EntityTypeConfiguration<CategoryAdditionalFields>
    {
        public CategoryAdditionalFieldsConfiguration()
        {
            ToTable("dbo.CategoryAdditionalFields");
            HasKey(x => x.CategoryAdditionalFieldId);

            Property(x => x.CategoryAdditionalFieldId).HasColumnName("CategoryAdditionalFieldId").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.CategoryId).HasColumnName("CategoryId").IsRequired();
            Property(x => x.AdditionalFieldId).HasColumnName("AdditionalFieldId").IsRequired();
            Property(x => x.Single).HasColumnName("Single").IsRequired();
            Property(x => x.Required).HasColumnName("Required").IsRequired();
            Property(x => x.Confidential).HasColumnName("Confidential").IsRequired();

            // Foreign keys
            HasRequired(a => a.Categories).WithMany(b => b.CategoryAdditionalFields).HasForeignKey(c => c.CategoryId); // FK_CategoryAdditionalFields_Categories
            HasRequired(a => a.AdditionalFields).WithMany(b => b.CategoryAdditionalFields).HasForeignKey(c => c.AdditionalFieldId); // FK_CategoryAdditionalFields_AdditionalFields
            InitializePartial();
        }
        partial void InitializePartial();
    }

    // DeletedPages
    internal partial class DeletedPagesConfiguration : EntityTypeConfiguration<DeletedPages>
    {
        public DeletedPagesConfiguration()
        {
            ToTable("dbo.DeletedPages");
            HasKey(x => x.DeletedPageId);

            Property(x => x.DeletedPageId).HasColumnName("DeletedPageId").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Active).HasColumnName("Active").IsRequired();
            Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            Property(x => x.EditedDate).HasColumnName("EditedDate").IsOptional();
            Property(x => x.DeletedDate).HasColumnName("DeletedDate").IsOptional();
            Property(x => x.DocumentId).HasColumnName("DocumentId").IsRequired();
            Property(x => x.Page).HasColumnName("Page").IsRequired();

            // Foreign keys
            HasRequired(a => a.Documents).WithMany(b => b.DeletedPages).HasForeignKey(c => c.DocumentId); // FK_DeletedPages_Documents
            InitializePartial();
        }
        partial void InitializePartial();
    }

    // Documents
    internal partial class DocumentsConfiguration : EntityTypeConfiguration<Documents>
    {
        public DocumentsConfiguration()
        {
            ToTable("dbo.Documents");
            HasKey(x => x.DocumentId);

            Property(x => x.DocumentId).HasColumnName("DocumentId").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Active).HasColumnName("Active").IsRequired();
            Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            Property(x => x.EditedDate).HasColumnName("EditedDate").IsOptional();
            Property(x => x.DeletedDate).HasColumnName("DeletedDate").IsOptional();
            Property(x => x.ExternalId).HasColumnName("ExternalId").IsRequired();
            Property(x => x.DocumentStatusId).HasColumnName("DocumentStatusId").IsRequired();

            // Foreign keys
            HasRequired(a => a.DocumentStatus).WithMany(b => b.Documents).HasForeignKey(c => c.DocumentStatusId); // FK_Documents_DocumentStatus
            InitializePartial();
        }
        partial void InitializePartial();
    }

    // DocumentStatus
    internal partial class DocumentStatusConfiguration : EntityTypeConfiguration<DocumentStatus>
    {
        public DocumentStatusConfiguration()
        {
            ToTable("dbo.DocumentStatus");
            HasKey(x => x.DocumentStatusId);

            Property(x => x.DocumentStatusId).HasColumnName("DocumentStatusId").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Active).HasColumnName("Active").IsRequired();
            Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            Property(x => x.EditedDate).HasColumnName("EditedDate").IsOptional();
            Property(x => x.DeletedDate).HasColumnName("DeletedDate").IsOptional();
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(255);
            InitializePartial();
        }
        partial void InitializePartial();
    }

    // RegisterEvents
    internal partial class RegisterEventsConfiguration : EntityTypeConfiguration<RegisterEvents>
    {
        public RegisterEventsConfiguration()
        {
            ToTable("dbo.RegisterEvents");
            HasKey(x => x.RegisterEventId);

            Property(x => x.RegisterEventId).HasColumnName("RegisterEventId").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Active).HasColumnName("Active").IsRequired();
            Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            Property(x => x.EditedDate).HasColumnName("EditedDate").IsOptional();
            Property(x => x.DeletedDate).HasColumnName("DeletedDate").IsOptional();
            Property(x => x.UserId).HasColumnName("UserId").IsRequired();
            Property(x => x.Identifier).HasColumnName("Identifier").IsRequired();
            Property(x => x.Type).HasColumnName("Type").IsRequired().HasMaxLength(100);
            Property(x => x.Source).HasColumnName("Source").IsRequired();
            Property(x => x.Text).HasColumnName("Text").IsRequired();
            InitializePartial();
        }
        partial void InitializePartial();
    }

    // SliceCategoryAdditionalFields
    internal partial class SliceCategoryAdditionalFieldsConfiguration : EntityTypeConfiguration<SliceCategoryAdditionalFields>
    {
        public SliceCategoryAdditionalFieldsConfiguration()
        {
            ToTable("dbo.SliceCategoryAdditionalFields");
            HasKey(x => x.SliceCategoryAdditionalFieldId);

            Property(x => x.SliceCategoryAdditionalFieldId).HasColumnName("SliceCategoryAdditionalFieldId").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Active).HasColumnName("Active").IsRequired();
            Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            Property(x => x.EditedDate).HasColumnName("EditedDate").IsOptional();
            Property(x => x.DeletedDate).HasColumnName("DeletedDate").IsOptional();
            Property(x => x.SliceId).HasColumnName("SliceId").IsRequired();
            Property(x => x.CategoryAdditionalFieldId).HasColumnName("CategoryAdditionalFieldId").IsRequired();
            Property(x => x.Value).HasColumnName("Value").IsRequired().HasMaxLength(255);

            // Foreign keys
            HasRequired(a => a.Slices).WithMany(b => b.SliceCategoryAdditionalFields).HasForeignKey(c => c.SliceId); // FK_SliceCategoryAdditionalFields_Slices
            HasRequired(a => a.CategoryAdditionalFields).WithMany(b => b.SliceCategoryAdditionalFields).HasForeignKey(c => c.CategoryAdditionalFieldId); // FK_SliceCategoryAdditionalFields_CategoryAdditionalFields
            InitializePartial();
        }
        partial void InitializePartial();
    }

    // SlicePages
    internal partial class SlicePagesConfiguration : EntityTypeConfiguration<SlicePages>
    {
        public SlicePagesConfiguration()
        {
            ToTable("dbo.SlicePages");
            HasKey(x => x.SlicePageId);

            Property(x => x.SlicePageId).HasColumnName("SlicePageId").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Active).HasColumnName("Active").IsRequired();
            Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            Property(x => x.EditedDate).HasColumnName("EditedDate").IsOptional();
            Property(x => x.DeletedDate).HasColumnName("DeletedDate").IsOptional();
            Property(x => x.SliceId).HasColumnName("SliceId").IsRequired();
            Property(x => x.Page).HasColumnName("Page").IsRequired();
            Property(x => x.Rotate).HasColumnName("Rotate").IsOptional();

            // Foreign keys
            HasRequired(a => a.Slices).WithMany(b => b.SlicePages).HasForeignKey(c => c.SliceId); // FK_SlicePages_Slices
            InitializePartial();
        }
        partial void InitializePartial();
    }

    // Slices
    internal partial class SlicesConfiguration : EntityTypeConfiguration<Slices>
    {
        public SlicesConfiguration()
        {
            ToTable("dbo.Slices");
            HasKey(x => x.SliceId);

            Property(x => x.SliceId).HasColumnName("SliceId").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Active).HasColumnName("Active").IsRequired();
            Property(x => x.CreatedDate).HasColumnName("CreatedDate").IsRequired();
            Property(x => x.EditedDate).HasColumnName("EditedDate").IsOptional();
            Property(x => x.DeletedDate).HasColumnName("DeletedDate").IsOptional();
            Property(x => x.DocumentId).HasColumnName("DocumentId").IsRequired();
            Property(x => x.CategoryId).HasColumnName("CategoryId").IsOptional();
            Property(x => x.Name).HasColumnName("Name").IsRequired().HasMaxLength(255);
            Property(x => x.Sent).HasColumnName("Sent").IsRequired();

            // Foreign keys
            HasRequired(a => a.Documents).WithMany(b => b.Slices).HasForeignKey(c => c.DocumentId); // FK_Slices_Documents
            HasOptional(a => a.Categories).WithMany(b => b.Slices).HasForeignKey(c => c.CategoryId); // FK_Slices_Categories
            InitializePartial();
        }
        partial void InitializePartial();
    }

}

