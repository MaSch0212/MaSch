﻿using System;
using System.Collections.Generic;

namespace MaSch.Console.Cli.Runtime
{
    public class CliArgumentParserResult
    {
        public bool Success { get; }
        public IEnumerable<CliError> Errors { get; }
        public ICliCommandInfo? Command { get; }
        public object? Options { get; }

        internal protected CliArgumentParserResult(IEnumerable<CliError> errors, ICliCommandInfo? command, object? options)
        {
            Success = false;
            Errors = errors;
            Command = command;
            Options = options;
        }

        internal protected CliArgumentParserResult(IEnumerable<CliError> errors)
            : this(errors, null, null)
        {
        }

        internal protected CliArgumentParserResult(ICliCommandInfo command, object options)
        {
            Success = true;
            Errors = Array.Empty<CliError>();
            Command = command;
            Options = options;
        }
    }
}
