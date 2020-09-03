using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.Extensions;
using Bit0.CrunchLog.Helpers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Bit0.CrunchLog
{
    public class ContentInitializer : IContentInitializer
    {
        private readonly CrunchConfig _siteConfig;
        private readonly ILogger<ContentInitializer> _logger;

        public ContentInitializer(CrunchConfig siteConfig, ILogger<ContentInitializer> logger)
        {
            _siteConfig = siteConfig;
            _logger = logger;
        }

        private void GenerateConfig()
        {
#if DEBUG
            Thread.Sleep(1000);
#endif
            _logger.LogInformation("Create new config file.");

            Console.CancelKeyPress += (sender, ev) =>
            {
                _logger.LogWarning("Input aborted by user");
                ev.Cancel = true;
                Environment.Exit(10000);
            };

            Console.WriteLine("Initialize new Crunchlog");
            Console.WriteLine();
            Console.WriteLine("This utility will walk you through creating a crunch.json file.");
            Console.WriteLine("It only covers the most common items, and tries to guess sensible defaults.");
            Console.WriteLine("See documentation on these fields and exactly what they do.");
            Console.WriteLine();

            _siteConfig.Title = ReadLine("Site Tilte: ");
            _siteConfig.SubTitle = ReadLine("Site Subtitle: ");
            _siteConfig.Description = ReadLine("Site Description: ");
            _siteConfig.BaseUrl = ReadLine("Base Url: ");

            var author = new Author();
            author.Name = ReadLine("Author Name: ");
            author.Alias = ReadLine("Author Alias: ");
            author.Email = ReadLine("Author Email: ");

            _siteConfig.Authors.Add(author.Alias, author);
            _siteConfig.Copyright = new Copyright
            {
                Owner = author.Name,
                StartYear = DateTime.Now.Year
            };

            var catTitle = "Uncatorized";
            _siteConfig.Categories.Add(catTitle, new CategoryInfo
            {
                Title = catTitle,
                Color = "#000000",
                Image = new SiteImage
                {
                    Url = $"https://dummyimage.com/1920x864/000/fff.png&text={catTitle}",
                    Width = 1920,
                    Height = 864,
                    Type = "image/png",
                    Placeholder = $"https://dummyimage.com/20x9/000/fff.png&text={catTitle}"
                },
                ShowInMainMenu = false,
                Description = "Uncategorized post",
                Permalink = String.Format(StaticKeys.CategoryPathFormat, catTitle)
            });
            _siteConfig.DefaultCategory = _siteConfig.Categories.First().Key;

            _siteConfig.DefaultBannerImage = new SiteImage
            {
                Url = "https://dummyimage.com/1920x864/09f/fff.png&text=Default+Image",
                Width = 1920,
                Height = 864,
                Type = "image/png",
                Placeholder = "https://dummyimage.com/20x9/09f/fff.png&text=Default+Image"
            };

            _siteConfig.Menu.Add("main", new List<MenuItem>
            {
                new MenuItem
                {
                    Title = "Home",
                    Url = "/",
                    Order = 100
                }
            });

            _siteConfig.Paths.BasePath
                .CreateFile("crunch.json", JsonConvert.SerializeObject(_siteConfig, Formatting.Indented));

            _logger.LogInformation("Created `crunch.json` file");
        }

        private void GenerateDirectory()
        {
            CreateDirecory(_siteConfig.Paths.ContentPath);
            CreateDirecory(_siteConfig.Paths.ContentPath.CombineDirPath("Posts"));
            CreateDirecory(_siteConfig.Paths.ContentPath.CombineDirPath("Pages"));
            CreateDirecory(_siteConfig.Paths.ContentPath.CombineDirPath("Drafts"));


            CreateDirecory(_siteConfig.Paths.ImagesPath);
            CreateDirecory(_siteConfig.Paths.AssetsPath);
            CreateDirecory(_siteConfig.Paths.PluginsPath);
            CreateDirecory(_siteConfig.Paths.ThemesPath);
        }

        private void GeneratePostDraft()
        {
            var date = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            var content = @$"---
title: ""Title""
slug: slug
datePublished: {date}
dateUpdated: {date}
tags:
- tags
categories:
- Uncatorized
published: false
intro: ""Intro""
---

Main Body

*Tips:* Start with a H2 ( ## ).
";

            _siteConfig.Paths.ContentPath
                .GetDirectories("Drafts", SearchOption.TopDirectoryOnly)
                .First()
                .CreateFile("0000-draft.md", content);

            _logger.LogInformation("Created a draft template");

        }

        private void CreateDirecory(DirectoryInfo dir)
        {
            dir.Create();
            var file = dir.CreateFile("notempty");

            _logger.LogInformation($"Created `{dir.Name}` directory");
        }

        public void Generate()
        {
            GenerateConfig();
            GenerateDirectory();
            GeneratePostDraft();
        }

        private String ReadLine(String text)
        {
            Console.Write(text);
            var input = Console.ReadLine();

            return String.IsNullOrWhiteSpace(input) ? throw new Exception("Input Error") : input;
        }
    }
}
