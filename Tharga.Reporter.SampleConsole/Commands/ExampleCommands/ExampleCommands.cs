﻿using Tharga.Toolkit.Console.Command.Base;

namespace Tharga.Reporter.ConsoleSample.Commands.ExampleCommands
{
    public class ExampleCommands : ContainerCommandBase
    {
        public ExampleCommands() 
            : base("example")
        {
            RegisterCommand(new ExampleInvoiceCommand());
            RegisterCommand(new ExampleArticleNoteCommand());
        }
    }
}