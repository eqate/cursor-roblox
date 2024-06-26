// do not touch anything below if you dont no what your doing.
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

class CursorUpdater
{
    const string Version = "1.0";
    const string VersionUrl = "https://raw.githubusercontent.com/eqate/cursor-roblox/main/version.txt";
    const string CursorFileName = "cursor.png";
    const string BloxstrapPathTemplate = @"C:\Users\{0}\AppData\Local\Bloxstrap\Versions\version-eb181506c14a4601\content\textures\Cursors\KeyboardMouse";
    const string SetupPathTemplate = @"C:\Users\{0}\Downloads\Setup\cursor.png";
    const string BloxstrapTexturePathTemplate = @"C:\Users\{0}\AppData\Local\Bloxstrap\Versions\version-eb181506c14a4601\content\textures";
    
    static async Task Main(string[] args)
    {
        string userName = Environment.UserName;
        string bloxstrapPath = string.Format(BloxstrapPathTemplate, userName);
        string cursorPath = string.Format(SetupPathTemplate, userName);
        string bloxstrapTexturePath = string.Format(BloxstrapTexturePathTemplate, userName);
        
        try
        {
            if (Directory.Exists(bloxstrapPath))
            {
                DirectoryInfo di = new DirectoryInfo(bloxstrapPath);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                Console.WriteLine("Cleared existing files in the KeyboardMouse directory.");
            }
            string[] cursorNames = { "ArrowCursor.png", "ArrowFarCursor.png", "IBeamCursor.png" };
            foreach (string cursorName in cursorNames)
            {
                string destPath = Path.Combine(bloxstrapPath, cursorName);
                File.Copy(cursorPath, destPath, true);
                Console.WriteLine($"copied - {cursorName}.");
            }

            // replace shiftlock
            string mouseLockedCursorPath = Path.Combine(bloxstrapTexturePath, "MouseLockedCursor.png");
            File.Copy(cursorPath, mouseLockedCursorPath, true);
            Console.WriteLine("replaced shiftlock with cursor");
            using (HttpClient client = new HttpClient())
            {
                string latestVersion = await client.GetStringAsync(VersionUrl);
                if (Version != latestVersion.Trim())
                {
                    Console.WriteLine("Check GitHub for the newest release!");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
