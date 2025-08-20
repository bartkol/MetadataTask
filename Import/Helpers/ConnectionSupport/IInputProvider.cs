namespace Import.Helpers.ConnectionSupport;

/// <summary>
/// Interface for input provider to handle user inputs.
/// Wrapper of Console.ReadLine() and similar methods.
/// </summary>
public interface IInputProvider
{
    /// <summary>
    /// Gets the Fivetran API Key from user input.
    /// </summary>
    /// <returns>API Key as a string</returns>
    public string GetApiKey();

    /// <summary>
    /// Gets the Fivetran API Secret from user input. 
    /// </summary>
    /// <returns>API Secret as a string</returns>
    public string GetApiSecret();
    
    /// <summary>
    /// Index of the group selected by the user.
    /// </summary>
    /// <returns>Group index as an int</returns>
    /// <exception cref="ArgumentException">Thrown when Group ID is not a valid integer.</exception>
    public int GetGroupIndex();
}