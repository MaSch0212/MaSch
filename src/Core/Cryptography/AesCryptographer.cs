using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace MaSch.Core.Cryptography;

/// <summary>
/// Represents a class with which data can be encrypted and decrypted with the AES256 encryption.
/// </summary>
public class AesCryptographer
{
    private const int KeySize = 256;
    private static readonly byte[] DefaultSaltBytes = { 147, 127, 196, 156, 78, 110, 38, 67, 181, 5, 167, 58, 2, 214, 216, 40 };

    private byte[] _initVector;

    /// <summary>
    /// Initializes a new instance of the <see cref="AesCryptographer"/> class.
    /// </summary>
    public AesCryptographer()
        : this(
              Guid.Parse(Assembly.GetEntryAssembly()?.GetCustomAttribute<GuidAttribute>()?.Value
                  ?? throw new InvalidOperationException("You need to either define a GuidAttribute on the assembly or use the other constructors.")))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AesCryptographer"/> class.
    /// </summary>
    /// <param name="initVector">The initialize vector.</param>
    public AesCryptographer(Guid initVector)
        : this(initVector.ToByteArray())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AesCryptographer"/> class.
    /// </summary>
    /// <param name="initVector">The initialize vector.</param>
    public AesCryptographer(byte[] initVector)
    {
        VerifyInitVector(initVector);
        _initVector = initVector;
    }

    /// <summary>
    /// Changes the initialize vector.
    /// </summary>
    /// <param name="initVector">The initialize vector.</param>
    /// <exception cref="ArgumentException">The init-vector has to be 16 bytes long.</exception>
    public void ChangeInitVector(byte[] initVector)
    {
        VerifyInitVector(initVector);
        _initVector = initVector;
    }

    /// <summary>
    /// Encrypts the specified plain text.
    /// </summary>
    /// <param name="plainText">The plain text.</param>
    /// <param name="passPhrase">The password phrase.</param>
    /// <param name="saltBytes">The salt bytes.</param>
    /// <returns>Returns the encrypted data.</returns>
    public byte[] Encrypt(string plainText, string passPhrase, byte[]? saltBytes = null)
    {
        _ = Guard.NotNull(plainText);
        _ = Guard.NotNull(passPhrase);

        using var plainTextStream = new MemoryStream(Encoding.UTF8.GetBytes(plainText));
        using var outStream = new MemoryStream();

        EncryptImpl(outStream, plainTextStream, passPhrase, saltBytes);
        return outStream.ToArray();
    }

    /// <summary>
    /// Encrypts the specified output stream and writes the encrypted data into the input stream.
    /// </summary>
    /// <param name="output">The output stream.</param>
    /// <param name="input">The input stream.</param>
    /// <param name="passPhrase">The password phrase.</param>
    /// <param name="saltBytes">The salt bytes.</param>
    public void Encrypt(Stream output, Stream input, string passPhrase, byte[]? saltBytes = null)
    {
        _ = Guard.NotNull(output);
        _ = Guard.NotNull(input);
        _ = Guard.NotNull(passPhrase);

        EncryptImpl(output, input, passPhrase, saltBytes);
    }

    /// <summary>
    /// Decrypts the specified bytes.
    /// </summary>
    /// <param name="bytes">The data to decrypt.</param>
    /// <param name="passPhrase">The password phrase.</param>
    /// <param name="saltBytes">The salt bytes.</param>
    /// <returns>Returns the decrypted data as UTF8 string.</returns>
    public string Decrypt(byte[] bytes, string passPhrase, byte[]? saltBytes = null)
    {
        _ = Guard.NotNull(bytes);
        _ = Guard.NotNull(passPhrase);

        using var inStream = new MemoryStream(bytes);
        using var outStream = new MemoryStream();

        DecryptImpl(outStream, inStream, passPhrase, saltBytes);
        return Encoding.UTF8.GetString(outStream.ToArray());
    }

    /// <summary>
    /// Decrypts the specified output stream and writes the decrypted data into the input stream.
    /// </summary>
    /// <param name="output">The output stream.</param>
    /// <param name="input">The input stream.</param>
    /// <param name="passPhrase">The password phrase.</param>
    /// <param name="saltBytes">The salt bytes.</param>
    public void Decrypt(Stream output, Stream input, string passPhrase, byte[]? saltBytes = null)
    {
        _ = Guard.NotNull(output);
        _ = Guard.NotNull(input);
        _ = Guard.NotNull(passPhrase);

        DecryptImpl(output, input, passPhrase, saltBytes);
    }

    private static void VerifyInitVector(byte[] initVector)
    {
        _ = Guard.NotNull(initVector);

        if (initVector.Length != 16)
            throw new ArgumentException("The init-vector has to be 16 bytes long.");
    }

    private void EncryptImpl(Stream output, Stream input, string passPhrase, byte[]? saltBytes)
    {
#if NETSTANDARD2_1_OR_GREATER || NET472_OR_GREATER || NETCOREAPP2_0_OR_GREATER
        using var password = new Rfc2898DeriveBytes(passPhrase, saltBytes ?? DefaultSaltBytes, 1000, HashAlgorithmName.SHA1);
#else
        using var password = new Rfc2898DeriveBytes(passPhrase, saltBytes ?? DefaultSaltBytes);
#endif
        var key = password.GetBytes(KeySize / 8);
        using var aes = Aes.Create();
        var encryptor = aes.CreateEncryptor(key, _initVector);
        using var encryptStream = new CryptoStream(output, encryptor, CryptoStreamMode.Write);
        int i;
        while ((i = input.ReadByte()) >= 0)
        {
            encryptStream.WriteByte((byte)i);
        }
    }

    private void DecryptImpl(Stream output, Stream input, string passPhrase, byte[]? saltBytes)
    {
#if NETSTANDARD2_1_OR_GREATER || NET472_OR_GREATER || NETCOREAPP2_0_OR_GREATER
        using var password = new Rfc2898DeriveBytes(passPhrase, saltBytes ?? DefaultSaltBytes, 1000, HashAlgorithmName.SHA1);
#else
        using var password = new Rfc2898DeriveBytes(passPhrase, saltBytes ?? DefaultSaltBytes);
#endif
        var key = password.GetBytes(KeySize / 8);
        using var aes = Aes.Create();
        var decryptor = aes.CreateDecryptor(key, _initVector);
        using var decryptStream = new CryptoStream(input, decryptor, CryptoStreamMode.Read);
        int i;
        while ((i = decryptStream.ReadByte()) >= 0)
        {
            output.WriteByte((byte)i);
        }
    }
}
