using MaSch.Core;
using MaSch.Core.Extensions;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MaSch.Data.Extensions
{
    /// <summary>
    /// Contains extensions for the <see cref="IDbConnection"/> interface.
    /// </summary>
    public static class DbConnectionExtensions
    {
        /// <summary>
        /// Creates a new <see cref="IDbCommand"/>.
        /// </summary>
        /// <param name="connection">The connection for which to create the command.</param>
        /// <param name="modifyCommandAction">A delegate with which the command can be modified before it gets returned.</param>
        /// <returns>Returns a new <see cref="IDbCommand"/> for this connection.</returns>
        public static IDbCommand CreateCommand(this IDbConnection connection, Action<IDbCommand>? modifyCommandAction)
            => CreateCommandImpl(connection, string.Empty, null, null, null, modifyCommandAction);

        /// <summary>
        /// Creates a new <see cref="IDbCommand"/>.
        /// </summary>
        /// <param name="connection">The connection for which to create the command.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="modifyCommandAction">A delegate with which the command can be modified before it gets returned.</param>
        /// <returns>Returns a new <see cref="IDbCommand"/> for this connection.</returns>
        public static IDbCommand CreateCommand(this IDbConnection connection, string commandText, Action<IDbCommand>? modifyCommandAction = null)
            => CreateCommandImpl(connection, commandText, null, null, null, modifyCommandAction);

        /// <summary>
        /// Creates a new <see cref="IDbCommand"/>.
        /// </summary>
        /// <param name="connection">The connection for which to create the command.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="modifyCommandAction">A delegate with which the command can be modified before it gets returned.</param>
        /// <returns>Returns a new <see cref="IDbCommand"/> for this connection.</returns>
        public static IDbCommand CreateCommand(this IDbConnection connection, string commandText, CommandType commandType, Action<IDbCommand>? modifyCommandAction = null)
            => CreateCommandImpl(connection, commandText, null, commandType, null, modifyCommandAction);

        /// <summary>
        /// Creates a new <see cref="IDbCommand"/>.
        /// </summary>
        /// <param name="connection">The connection for which to create the command.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="modifyCommandAction">A delegate with which the command can be modified before it gets returned.</param>
        /// <returns>Returns a new <see cref="IDbCommand"/> for this connection.</returns>
        public static IDbCommand CreateCommand(this IDbConnection connection, string commandText, int timeout, Action<IDbCommand>? modifyCommandAction = null)
            => CreateCommandImpl(connection, commandText, null, null, timeout, modifyCommandAction);

        /// <summary>
        /// Creates a new <see cref="IDbCommand"/>.
        /// </summary>
        /// <param name="connection">The connection for which to create the command.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="modifyCommandAction">A delegate with which the command can be modified before it gets returned.</param>
        /// <returns>Returns a new <see cref="IDbCommand"/> for this connection.</returns>
        public static IDbCommand CreateCommand(this IDbConnection connection, string commandText, CommandType commandType, int timeout, Action<IDbCommand>? modifyCommandAction = null)
            => CreateCommandImpl(connection, commandText, null, commandType, timeout, modifyCommandAction);

        /// <summary>
        /// Creates a new <see cref="IDbCommand"/>.
        /// </summary>
        /// <param name="connection">The connection for which to create the command.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="addParametersAction">A delegate that can be used to add parameters to the command.</param>
        /// <param name="modifyCommandAction">A delegate with which the command can be modified before it gets returned.</param>
        /// <returns>Returns a new <see cref="IDbCommand"/> for this connection.</returns>
        public static IDbCommand CreateCommand(this IDbConnection connection, string commandText, Action<DbCommandParameterCollection>? addParametersAction, Action<IDbCommand>? modifyCommandAction = null)
            => CreateCommandImpl(connection, commandText, addParametersAction, null, null, modifyCommandAction);

        /// <summary>
        /// Creates a new <see cref="IDbCommand"/>.
        /// </summary>
        /// <param name="connection">The connection for which to create the command.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="addParametersAction">A delegate that can be used to add parameters to the command.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="modifyCommandAction">A delegate with which the command can be modified before it gets returned.</param>
        /// <returns>Returns a new <see cref="IDbCommand"/> for this connection.</returns>
        public static IDbCommand CreateCommand(this IDbConnection connection, string commandText, Action<DbCommandParameterCollection>? addParametersAction, CommandType commandType, Action<IDbCommand>? modifyCommandAction = null)
            => CreateCommandImpl(connection, commandText, addParametersAction, commandType, null, modifyCommandAction);

        /// <summary>
        /// Creates a new <see cref="IDbCommand"/>.
        /// </summary>
        /// <param name="connection">The connection for which to create the command.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="addParametersAction">A delegate that can be used to add parameters to the command.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="modifyCommandAction">A delegate with which the command can be modified before it gets returned.</param>
        /// <returns>Returns a new <see cref="IDbCommand"/> for this connection.</returns>
        public static IDbCommand CreateCommand(this IDbConnection connection, string commandText, Action<DbCommandParameterCollection>? addParametersAction, int timeout, Action<IDbCommand>? modifyCommandAction = null)
            => CreateCommandImpl(connection, commandText, addParametersAction, null, timeout, modifyCommandAction);

        /// <summary>
        /// Creates a new <see cref="IDbCommand"/>.
        /// </summary>
        /// <param name="connection">The connection for which to create the command.</param>
        /// <param name="commandText">The command text.</param>
        /// <param name="addParametersAction">A delegate that can be used to add parameters to the command.</param>
        /// <param name="commandType">Type of the command.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="modifyCommandAction">A delegate with which the command can be modified before it gets returned.</param>
        /// <returns>Returns a new <see cref="IDbCommand"/> for this connection.</returns>
        public static IDbCommand CreateCommand(this IDbConnection connection, string commandText, Action<DbCommandParameterCollection>? addParametersAction, CommandType commandType, int timeout, Action<IDbCommand>? modifyCommandAction = null)
            => CreateCommandImpl(connection, commandText, addParametersAction, commandType, timeout, modifyCommandAction);

        private static IDbCommand CreateCommandImpl(IDbConnection connection, string commandText, Action<DbCommandParameterCollection>? addParametersAction, CommandType? commandType, int? timeout, Action<IDbCommand>? modifyCommandAction)
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

        /// <summary>
        /// Opens the connection if it was not already opened.
        /// </summary>
        /// <param name="connection">The connection to open.</param>
        public static void OpenIfNecessary(this IDbConnection connection)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
        }

        /// <summary>
        /// Opens the connection if it was not already opened.
        /// </summary>
        /// <param name="connection">The connection to open.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task OpenIfNecessaryAsync(this IDbConnection connection)
        {
            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();
        }

        /// <summary>
        /// Represents a collection of parameters for a <see cref="IDbCommand"/>.
        /// </summary>
        public class DbCommandParameterCollection
        {
            /// <summary>
            /// Gets the command to which the parameters are added.
            /// </summary>
            public IDbCommand? Command { get; private set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="DbCommandParameterCollection"/> class.
            /// </summary>
            /// <param name="command">The command to which the parameters should be added.</param>
            public DbCommandParameterCollection(IDbCommand command)
            {
                Guard.NotNull(command, nameof(command));
                Command = command;
            }

            /// <summary>
            /// Adds the specified parameter to the <see cref="IDbCommand"/>.
            /// </summary>
            /// <param name="paramName">Name of the parameter.</param>
            /// <param name="paramValue">The parameter value.</param>
            /// <param name="modifyParameterAction">a delegate with which the parameter can be modified before it gets added.</param>
            /// <returns>A reference to this instance after the add operation has completed.</returns>
            /// <exception cref="InvalidOperationException">The collection has been closed.</exception>
            public DbCommandParameterCollection Add(string paramName, object paramValue, Action<IDbDataParameter>? modifyParameterAction = null)
            {
                if (Command == null)
                    throw new InvalidOperationException("The collection has been closed.");

                var param = Command.CreateParameter();
                param.ParameterName = paramName;
                param.Value = paramValue;
                modifyParameterAction?.Invoke(param);
                Command.Parameters.Add(param);

                return this;
            }

            /// <summary>
            /// Adds the specified parameters to the <see cref="IDbCommand"/>.
            /// </summary>
            /// <param name="parameters">The parameters to add.</param>
            /// <returns>A reference to this instance after the add operation has completed.</returns>
            /// <exception cref="InvalidOperationException">The collection has been closed.</exception>
            public DbCommandParameterCollection Add(params (string ParamName, object ParamValue)[] parameters)
            {
                parameters.ForEach(x => Add(x.ParamName, x.ParamValue));
                return this;
            }

            /// <summary>
            /// Closes this instance.
            /// </summary>
            internal void Close()
            {
                Command = null;
            }
        }
    }
}
