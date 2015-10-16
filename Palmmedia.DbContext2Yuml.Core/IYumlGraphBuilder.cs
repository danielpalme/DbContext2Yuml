namespace Palmmedia.DbContext2Yuml.Core
{
    /// <summary>
    /// Interface to create Yuml graphs from GIT History.
    /// </summary>
    public interface IYumlGraphBuilder
    {
        /// <summary>
        /// Creates the Yuml graph.
        /// </summary>
        /// <param name="pathToDll">The path to the DLL.</param>
        /// <returns>The Yuml graph.</returns>
        string CreateYumlGraph(string pathToDll);

        /// <summary>
        /// Creates the Yuml graph.
        /// </summary>
        /// <param name="pathToDll">The path to the DLL.</param>
        /// <param name="showInheritance">if set to <c>true</c> inheritance relations are rendered.</param>
        /// <returns>
        /// The Yuml graph.
        /// </returns>
        string CreateYumlGraph(string pathToDll, bool showInheritance);
    }
}
