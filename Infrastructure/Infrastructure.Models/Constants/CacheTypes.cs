namespace Infrastructure.Models.Constants
{
    public enum CacheTypes
    {
        /// <summary>
        /// caching files
        /// </summary>
        FileSystem = 1,

        /// <summary>
        /// caching database queries / storedproc/ tables
        /// </summary>
        Database = 2,

        /// <summary>
        /// caching api calls and thirdparties calls
        /// </summary>
        Network = 3
    }
}
