using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Oonyoen.CommandLineParser.AspNetCore
{
    public class InteractiveCommandLineOptions
    {
        /// <summary>
        /// The verb types and their handlers.
        /// </summary>
        public Dictionary<Type, Action<ParserResult<object>, IServiceProvider>> Verbs { get; set; } = new();

        /// <summary>
        /// The error handler for unparsed values.
        /// </summary>
        public Action<ParserResult<object>, IServiceProvider> ErrorHandler { get; set; }

        private void AddVerb(Type type, Action<ParserResult<object>, IServiceProvider> handler)
        {
            if (Attribute.IsDefined(type, typeof(VerbAttribute)))
            {
                Verbs.Add(type, handler);
            }
            else
            {
                throw new ArgumentException($"'{type.FullName}' must have the '{nameof(VerbAttribute)}' attribute.", nameof(type));
            }
        }

        public void AddVerb<TVerb>(Action<IServiceProvider, TVerb> handler)
        {
            AddVerb(typeof(TVerb), (parserResult, services) =>
            {
                parserResult.WithParsed<TVerb>(result => handler(services, result));
            });
        }

        public void AddVerb<TVerb>()
        {
            AddVerb(typeof(TVerb), (parserResult, services) =>
            {
                var handler = services.GetRequiredService<IVerbHandler<TVerb>>();
                parserResult.WithParsed<TVerb>(handler.Handle);
            });
        }

        public void AddError(Action<IServiceProvider, IEnumerable<Error>> handler)
        {
            ErrorHandler = (parserResult, services) =>
            {
                parserResult.WithNotParsed(errors => handler(services, errors));
            };
        }
    }
}
