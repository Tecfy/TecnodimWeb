

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
        IDbSet<ApiUsers> ApiUsers { get; set; } // ApiUsers
        IDbSet<AspNetRoles> AspNetRoles { get; set; } // AspNetRoles
        IDbSet<AspNetUserClaims> AspNetUserClaims { get; set; } // AspNetUserClaims
        IDbSet<AspNetUserLogins> AspNetUserLogins { get; set; } // AspNetUserLogins
        IDbSet<AspNetUserRoles> AspNetUserRoles { get; set; } // AspNetUserRoles
        IDbSet<AspNetUsers> AspNetUsers { get; set; } // AspNetUsers
        IDbSet<RegisterEvents> RegisterEvents { get; set; } // RegisterEvents

        int SaveChanges();
    }

    // ************************************************************************
    // Database context
    public partial class DBContext : DbContext, IDBContext
    {
        public IDbSet<ApiUsers> ApiUsers { get; set; } // ApiUsers
        public IDbSet<AspNetRoles> AspNetRoles { get; set; } // AspNetRoles
        public IDbSet<AspNetUserClaims> AspNetUserClaims { get; set; } // AspNetUserClaims
        public IDbSet<AspNetUserLogins> AspNetUserLogins { get; set; } // AspNetUserLogins
        public IDbSet<AspNetUserRoles> AspNetUserRoles { get; set; } // AspNetUserRoles
        public IDbSet<AspNetUsers> AspNetUsers { get; set; } // AspNetUsers
        public IDbSet<RegisterEvents> RegisterEvents { get; set; } // RegisterEvents

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

            modelBuilder.Configurations.Add(new ApiUsersConfiguration());
            modelBuilder.Configurations.Add(new AspNetRolesConfiguration());
            modelBuilder.Configurations.Add(new AspNetUserClaimsConfiguration());
            modelBuilder.Configurations.Add(new AspNetUserLoginsConfiguration());
            modelBuilder.Configurations.Add(new AspNetUserRolesConfiguration());
            modelBuilder.Configurations.Add(new AspNetUsersConfiguration());
            modelBuilder.Configurations.Add(new RegisterEventsConfiguration());
        OnModelCreatingPartial(modelBuilder);
        }

        partial void InitializePartial();
        partial void OnModelCreatingPartial(DbModelBuilder modelBuilder);
    }

    // ************************************************************************
    // POCO classes

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


    // ************************************************************************
    // POCO Configuration

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

}

