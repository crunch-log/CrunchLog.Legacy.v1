using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bit0.CrunchLog.Config;
using Bit0.CrunchLog.ContentTypes;
using Newtonsoft.Json;

namespace Bit0.CrunchLog.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var jsonSerializer = new JsonSerializer();
            var basePath = new DirectoryInfo(args[0]);
            var configFile = basePath.GetFiles("crunch.json", SearchOption.TopDirectoryOnly).SingleOrDefault();

            var config = new CrunchConfig();
            jsonSerializer.Populate(configFile?.OpenText(), config);
            //var config1 = (CrunchConfig)new JsonSerializer().Deserialize(configFile?.OpenText(), typeof(CrunchConfig));

            var outputPath = new DirectoryInfo(Path.Combine(basePath.FullName, config.Paths[CrunchConfigKeys.Output]));

            var postsPath = new DirectoryInfo(Path.Combine(basePath.FullName, config.Paths[CrunchConfigKeys.Posts]));

            var posts = new List<PostType>();

            foreach (var postfile in postsPath.GetFiles("*.json", SearchOption.AllDirectories))
            {
                var post = new PostType(postfile);
                jsonSerializer.Populate(postfile?.OpenText(), post);

                posts.Add(post);
            }

#if DEBUG
            Console.ReadKey();
#endif
        }
    }
}
