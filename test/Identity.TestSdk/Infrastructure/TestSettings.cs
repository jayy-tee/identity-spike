namespace Identity.TestSdk.Infrastructure
{
    public class TestSettings
    {
        public TestSettings()
        {
            EnvironmentVariables = new EnvironmentVariableDictionary();
        }

        /// <summary>
        /// Base URL to target. 
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// If TRUE, the API will be loaded 'in process' - this allows us to override dependency injection and debug from the test through to the code base. 
        /// </summary>
        public bool InProcess { get; set; }

        /// <summary>
        /// Environment variables to be set (process-wide) before the test is executed. 
        /// </summary>
        public EnvironmentVariableDictionary EnvironmentVariables { get; set; }
    }
}
