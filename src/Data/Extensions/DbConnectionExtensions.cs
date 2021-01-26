using System;
using System.Data;
using System.Threading.Tasks;
using MaSch.Common;
using MaSch.Common.Extensions;

namespace MaSch.Data.Extensions
{
    public static class DbConnectionExtensions
    {
        public static IDbCommand CreateCommand(this IDbConnection connection, Action<IDbCommand> modifyCommandAction)
            => CreateCommandImpl(connection, string.Empty, null, null, null, modifyCommandAction);
        public static IDbCommand CreateCommand(this IDbConnection connection, string commandText, Action<IDbCommand> modifyCommandAction = null)
            => CreateCommandImpl(connection, commandText, null, null, null, modifyCommandAction);
        public static IDbCommand CreateCommand(this IDbConnection connection, string commandText, CommandType commandType, Action<IDbCommand> modifyCommandAction = null)
            => CreateCommandImpl(connection, commandText, null, commandType, null, modifyCommandAction);
        public static IDbCommand CreateCommand(this IDbConnection connection, string commandText, int timeout, Action<IDbCommand> modifyCommandAction = null)
            => CreateCommandImpl(connection, commandText, null, null, timeout, modifyCommandAction);
        public static IDbCommand CreateCommand(this IDbConnection connection, string commandText, CommandType commandType, int timeout, Action<IDbCommand> modifyCommandAction = null)
            => CreateCommandImpl(connection, commandText, null, commandType, timeout, modifyCommandAction);
        public static IDbCommand CreateCommand(this IDbConnection connection, string commandText, Action<DbCommandParameterCollection> addParametersAction, Action<IDbCommand> modifyCommandAction = null)
            => CreateCommandImpl(connection, commandText, addParametersAction, null, null, modifyCommandAction);
        public static IDbCommand CreateCommand(this IDbConnection connection, string commandText, Action<DbCommandParameterCollection> addParametersAction, CommandType commandType, Action<IDbCommand> modifyCommandAction = null)
            => CreateCommandImpl(connection, commandText, addParametersAction, commandType, null, modifyCommandAction);
        public static IDbCommand CreateCommand(this IDbConnection connection, string commandText, Action<DbCommandParameterCollection> addParametersAction, int timeout, Action<IDbCommand> modifyCommandAction = null)
            => CreateCommandImpl(connection, commandText, addParametersAction, null, timeout, modifyCommandAction);
        public static IDbCommand CreateCommand(this IDbConnection connection, string commandText, Action<DbCommandParameterCollection> addParametersAction, CommandType commandType, int timeout, Action<IDbCommand> modifyCommandAction = null)
            => CreateCommandImpl(connection, commandText, addParametersAction, commandType, timeout, modifyCommandAction);
        private static IDbCommand CreateCommandImpl(IDbConnection connection, string commandText, Action<DbCommandParameterCollection> addParametersAction, CommandType? commandType, int? timeout, Action<IDbCommand> modifyCommandAction)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = commandText;
            if (commandType.HasValue)
                cmd.CommandType = commandType.Value;
            if (timeout.HasValue)
                cmd.CommandTimeout = timeout.Value;
            if (addParametersAction != null)
            {
                var collection = new DbCommandParameterCollection(cmd);
                addParametersAction(collection);
                collection.Close();
            }
            modifyCommandAction?.Invoke(cmd);

            return cmd;
        }

        public static void OpenIfNecessary(this IDbConnection connection)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
        }
        public static async Task OpenIfNecessaryAsync(this IDbConnection connection)
        {
            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();
        }

        public class DbCommandParameterCollection
        {
            public IDbCommand Command { get; private set; } 

            public DbCommandParameterCollection(IDbCommand command)
            {
                Guard.NotNull(command, nameof(command));
                Command = command;
            }

            public DbCommandParameterCollection Add(string paramName, object paramValue, Action<IDbDataParameter> modifyParameterAction = null)
            {
                if(Command == null)
                    throw new InvalidOperationException("The collection had been closed.");

                var param = Command.CreateParameter();
                param.ParameterName = paramName;
                param.Value = paramValue;
                modifyParameterAction?.Invoke(param);
                Command.Parameters.Add(param);

                return this;
            }

            public DbCommandParameterCollection Add(params (string paramName, object paramValue)[] parameters)
            {
                parameters.ForEach(x => Add(x.paramName, x.paramValue));
                return this;
            }

            internal void Close()
            {
                Command = null;
            }
        }
    }
}
