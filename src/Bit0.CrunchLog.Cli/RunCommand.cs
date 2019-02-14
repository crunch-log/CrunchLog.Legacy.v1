using Bit0.CrunchLog.Cli.Extensions;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Unosquare.Labs.EmbedIO;
using Unosquare.Swan;

namespace Bit0.CrunchLog.Cli
{
    [Command(CliOptionKeys.RunCommand, Description = CliOptionKeys.RunCommandDescription)]
    public class RunCommand : CliBase
    {
        [Argument(0, Description = CliOptionKeys.BasePathDescription)]
        [DirectoryExists]
        private String BasePath { get; } = "";

        [Argument(1, Description = CliOptionKeys.UrlDescription)]
        [DirectoryExists]
        private String Url { get; } = "";

        [Option(CliOptionKeys.VerboseTemplate, Description = CliOptionKeys.VerboseDescription)]
        private LogLevel VerboseLevel { get; } = LogLevel.Information;

        protected override Int32 OnExecute(CommandLineApplication app)
        {
            var args = app.BuildArguments(BasePath, VerboseLevel, Url);
            return app.Execute(args, (provider, logger, config) =>
            {
                logger.LogDebug("Run");
                var generator = provider.GetService<IContentGenerator>();
                generator.CleanOutput();
                generator.Publish();

                Terminal.OnLogMessageReceived += (s, e) =>
                {
                    LogLevel level;

                    switch (e.MessageType)
                    {
                        case LogMessageType.None:
                            level = LogLevel.None;
                            break;
                        case LogMessageType.Info:
                            level = LogLevel.Information;
                            break;
                        case LogMessageType.Debug:
                            level = LogLevel.Debug;
                            break;
                        case LogMessageType.Trace:
                            level = LogLevel.Trace;
                            break;
                        case LogMessageType.Error:
                            level = LogLevel.Error;
                            break;
                        case LogMessageType.Warning:
                            level = LogLevel.Warning;
                            break;
                        case LogMessageType.Fatal:
                            level = LogLevel.Critical;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    logger.Log(level, new EventId(2001, e.Source), $"[{e.Source}] {e.Message}", e.Exception, (state, exception) => state.ToString());
                };
                Terminal.Settings.DisplayLoggingMessageType = LogMessageType.None;

                var server = WebServer
                    .Create(args.Url)
                    .EnableCors()
                    .WithLocalSession()
                    .WithStaticFolderAt(config.Paths.OutputPath.FullName);

                server.OnNotFound = (c) =>
                {
                    logger.Log(LogLevel.Error, new EventId(2002, "Not Found"), $"Cannot find {c.Request.Url}");

                    return Task.FromResult(true);
                };

                var cts = new CancellationTokenSource();
                var task = server.RunAsync(cts.Token);

                app.OpenBrowser(args.Url);

                Console.WriteLine("Press any key to close server.");
                Console.ReadKey(true);
                cts.Cancel();

                try
                {
                    task.Wait(cts.Token);
                }
                catch (AggregateException)
                {
                    // We'd also actually verify the exception cause was that the task
                    // was canceled.
                }
                finally
                {
                    cts.Dispose();
                }
            });
        }
    }
}