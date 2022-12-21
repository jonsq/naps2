using NAPS2.Scan;

namespace NAPS2.ImportExport.Email.Mapi;

public class MapiDispatcher
{
    private readonly ScanningContext _scanningContext;
    private readonly IMapiWrapper _mapiWrapper;

#if NET6_0_OR_GREATER
    [System.Runtime.Versioning.SupportedOSPlatform("windows7.0")]
#endif
    public MapiDispatcher(ScanningContext scanningContext)
        : this(scanningContext, new MapiWrapper(new SystemEmailClients()))
    {
    }

    public MapiDispatcher(ScanningContext scanningContext, IMapiWrapper mapiWrapper)
    {
        _scanningContext = scanningContext;
        _mapiWrapper = mapiWrapper;
    }

    private bool UseWorker => Environment.Is64BitProcess;

    /// <summary>
    /// Sends an email described by the given message object.
    /// </summary>
    /// <param name="clientName">The MAPI client name.</param>
    /// <param name="message">The object describing the email message.</param>
    /// <returns>The MAPI return code.</returns>
    public async Task<MapiSendMailReturnCode> SendEmail(string? clientName, EmailMessage message)
    {
#if NET6_0_OR_GREATER
        if (!OperatingSystem.IsWindowsVersionAtLeast(7)) throw new InvalidOperationException("Windows-only");
#endif
        // TODO: We should always do this in a worker (64 or 32 bit). Specifically, loading the outlook library does something weird to WinForms such that "new Eto.Forms.TextArea()" errors with a missing office dll.
        if (UseWorker && !_mapiWrapper.CanLoadClient(clientName))
        {
            if (_scanningContext.WorkerFactory == null)
            {
                throw new InvalidOperationException(
                    "ScanningContext.WorkerFactory must be set to use MAPI from a 64-bit process.");
            }
            using var worker = _scanningContext.WorkerFactory.Create();
            return await worker.Service.SendMapiEmail(clientName, message);
        }
        return await _mapiWrapper.SendEmail(clientName, message);
    }
}