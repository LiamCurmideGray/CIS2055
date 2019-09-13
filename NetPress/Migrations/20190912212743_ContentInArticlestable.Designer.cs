﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetPress.Models;

namespace NetPress.Migrations
{
    [DbContext(typeof(UserAccountsContext))]
    [Migration("20190912212743_ContentInArticlestable")]
    partial class ContentInArticlestable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NetPress.Models.ArticleCategories", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CategoryName");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("NetPress.Models.Articles", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AuthorId");

                    b.Property<int?>("CategoriesId");

                    b.Property<int>("CategoryId");

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("LastModified");

                    b.Property<string>("Status");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CategoriesId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("NetPress.Models.Roles", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("RoleType");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("NetPress.Models.UserAccounts", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ArticlesId");

                    b.Property<int>("RoleId");

                    b.Property<string>("UserEmail")
                        .IsRequired();

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.Property<string>("UserPassword")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ArticlesId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserAccounts");
                });

            modelBuilder.Entity("NetPress.Models.Articles", b =>
                {
                    b.HasOne("NetPress.Models.ArticleCategories", "Categories")
                        .WithMany()
                        .HasForeignKey("CategoriesId");
                });

            modelBuilder.Entity("NetPress.Models.UserAccounts", b =>
                {
                    b.HasOne("NetPress.Models.Articles")
                        .WithMany("User")
                        .HasForeignKey("ArticlesId");

                    b.HasOne("NetPress.Models.Roles", "Role")
                        .WithMany("UserAccounts")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
