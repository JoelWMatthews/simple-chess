using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace SimpleChess.Game
{
    public class StockfishCommunicator : MonoBehaviour
    {
        public static StockfishCommunicator Instance { get; private set; }
        private Process stockfishProcess;
        private StreamWriter stockfishInput;
        private StreamReader stockfishOutput;

        private void Start()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            StartStockfish();
        }

        private void OnDestroy()
        {
            StopStockfish();
        }

        private void StartStockfish()
        {
            stockfishProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "Assets/Resources/Stockfish/stockfish-windows-x86-64-avx2.exe",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            stockfishProcess.Start();
            stockfishInput = stockfishProcess.StandardInput;
            stockfishOutput = stockfishProcess.StandardOutput;

            SendCommand("uci");
        }

        private void StopStockfish()
        {
            if (stockfishProcess != null && !stockfishProcess.HasExited)
            {
                SendCommand("quit");
                stockfishProcess.WaitForExit();
                stockfishProcess.Close();
            }
        }

        public void SendCommand(string command)
        {
            if (stockfishProcess != null && !stockfishProcess.HasExited)
            {
                stockfishInput.WriteLine(command);
                stockfishInput.Flush();
            }
        }

        public async Task<string> GetResponse()
        {
            if (stockfishProcess != null && !stockfishProcess.HasExited)
            {
                return await stockfishOutput.ReadLineAsync();
            }

            return null;
        }

        public async Task<string> GetBestMove(string fen)
        {
            SendCommand($"position fen {fen}");
            SendCommand("go movetime 1000");

            string response;
            while ((response = await GetResponse()) != null)
            {
                if (response.StartsWith("bestmove"))
                {
                    return response;
                }
            }

            return null;
        }
    }
}
