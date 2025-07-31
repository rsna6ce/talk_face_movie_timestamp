// Form1.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using NAudio.Wave;

namespace TalkFaceMovieTimestamp
{
    public partial class Form1 : Form
    {
        private WaveOutEvent waveOut; // NAudioの再生デバイス
        private AudioFileReader audioFileReader; // WAVファイルリーダー
        private List<(double from, double to)> intervals; // 2人目の発話区間
        private double? markStart; // 長押し開始時間
        private string wavePath; // 選択中のWAVファイルパス
        private ListBox lstTimestamps; // タイムスタンプ表示用リストボックス
        private Label lblTimer; // タイムカウンター
        private Label lblWaveFile; // WAVファイル名表示
        private Timer timer; // 再生位置更新用タイマー
        private bool isVerifyMode; // 検証再生モードフラグ
        private int currentIntervalIndex; // 現在の検証再生区間インデックス
        private Button btnFromMinus, btnFromPlus, btnToMinus, btnToPlus; // 編集ボタン

        public Form1()
        {
            InitializeComponent();
            intervals = new List<(double from, double to)>();
            isVerifyMode = false;
            currentIntervalIndex = -1;
            this.Height = 500; // フォームの高さを増やす
            SetupUI();
            SetupTimer();
            this.FormClosing += (s, e) => CleanupAudio(); // フォーム終了時にリソース解放
        }

        /// <summary>
        /// UIコンポーネントを初期化
        /// </summary>
        private void SetupUI()
        {
            // ボタン配置
            var btnLoad = new Button { Text = "WAV選択", Location = new Point(10, 10), Width = 100 };
            var btnPlay = new Button { Text = "再生/停止", Location = new Point(10, 50), Width = 100 };
            var btnVerify = new Button { Text = "検証再生", Location = new Point(120, 50), Width = 100 };
            var btnMark = new Button { Text = "2人目マーク (長押し)", Location = new Point(10, 90), Width = 210, Height = 50 };
            var btnLoadCsv = new Button { Text = "CSV読込", Location = new Point(10, 140), Width = 100 };
            var btnSave = new Button { Text = "CSV保存", Location = new Point(120, 140), Width = 100 };

            // WAVファイル名ラベル
            lblWaveFile = new Label
            {
                Location = new Point(120, 10),
                Size = new Size(210, 20),
                Font = new Font("Consolas", 10),
                Text = "未選択"
            };

            // リストボックス配置
            lstTimestamps = new ListBox
            {
                Location = new Point(10, 200),
                Size = new Size(210, 150),
                Font = new Font("Consolas", 10),
                HorizontalScrollbar = true
            };

            // タイムカウンター
            lblTimer = new Label
            {
                Location = new Point(10, 360),
                Size = new Size(210, 20),
                Font = new Font("Consolas", 10),
                Text = "00:00.000"
            };

            // 編集ボタン
            btnFromMinus = new Button { Text = "from <", Location = new Point(10, 390), Width = 50, Enabled = false };
            btnFromPlus = new Button { Text = "from >", Location = new Point(70, 390), Width = 50, Enabled = false };
            btnToMinus = new Button { Text = "to <", Location = new Point(130, 390), Width = 50, Enabled = false };
            btnToPlus = new Button { Text = "to >", Location = new Point(190, 390), Width = 50, Enabled = false };

            // イベントハンドラ
            btnLoad.Click += BtnLoad_Click;
            btnLoadCsv.Click += BtnLoadCsv_Click;
            btnSave.Click += BtnSave_Click;
            btnPlay.Click += BtnPlay_Click;
            btnVerify.Click += BtnVerify_Click;
            btnMark.MouseDown += BtnMark_MouseDown;
            btnMark.MouseUp += BtnMark_MouseUp;
            lstTimestamps.SelectedIndexChanged += LstTimestamps_SelectedIndexChanged;
            btnFromMinus.Click += (s, e) => AdjustInterval(lstTimestamps.SelectedIndex, -0.1, true);
            btnFromPlus.Click += (s, e) => AdjustInterval(lstTimestamps.SelectedIndex, 0.1, true);
            btnToMinus.Click += (s, e) => AdjustInterval(lstTimestamps.SelectedIndex, -0.1, false);
            btnToPlus.Click += (s, e) => AdjustInterval(lstTimestamps.SelectedIndex, 0.1, false);

            Controls.AddRange(new Control[] { btnLoad, btnLoadCsv, btnSave, btnPlay, btnVerify, btnMark, lstTimestamps, lblTimer, lblWaveFile, btnFromMinus, btnFromPlus, btnToMinus, btnToPlus });
        }

        /// <summary>
        /// リストボックスの選択変更
        /// </summary>
        private void LstTimestamps_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool hasSelection = lstTimestamps.SelectedIndex >= 0;
            btnFromMinus.Enabled = hasSelection;
            btnFromPlus.Enabled = hasSelection;
            btnToMinus.Enabled = hasSelection;
            btnToPlus.Enabled = hasSelection;
        }

        /// <summary>
        /// 区間の from または to を調整
        /// </summary>
        private void AdjustInterval(int index, double delta, bool isFrom)
        {
            if (index < 0 || index >= intervals.Count || audioFileReader == null)
                return;

            var (from, to) = intervals[index];
            double newFrom = from, newTo = to;
            double maxDuration = audioFileReader.TotalTime.TotalSeconds;

            if (isFrom)
            {
                newFrom = from + delta;
                if (newFrom < 0 || newFrom >= to)
                    return;
            }
            else
            {
                newTo = to + delta;
                if (newTo > maxDuration || newTo <= from)
                    return;
            }

            intervals[index] = (newFrom, newTo);
            lstTimestamps.Items[index] = FormatTimestamp(newFrom, newTo);
            lstTimestamps.TopIndex = Math.Max(0, index - lstTimestamps.Height / lstTimestamps.ItemHeight + 1);
        }

        /// <summary>
        /// 再生位置更新用タイマーを設定
        /// </summary>
        private void SetupTimer()
        {
            timer = new Timer
            {
                Interval = 50 // 50msごとに更新（20fps相当）
            };
            timer.Tick += (s, e) => UpdateTimer();
        }

        /// <summary>
        /// WAVファイルを選択して読み込み
        /// </summary>
        private void BtnLoad_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog { Filter = "WAVファイル|*.wav" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    wavePath = ofd.FileName;
                    CleanupAudio();
                    try
                    {
                        audioFileReader = new AudioFileReader(wavePath);
                        waveOut = new WaveOutEvent();
                        waveOut.Init(audioFileReader);
                        waveOut.PlaybackStopped += WaveOut_PlaybackStopped;
                        intervals.Clear();
                        lstTimestamps.Items.Clear();
                        lstTimestamps.ClearSelected();
                        lblTimer.Text = "00:00.000";
                        lblWaveFile.Text = Path.GetFileName(wavePath);
                        isVerifyMode = false;
                        currentIntervalIndex = -1;
                        btnFromMinus.Enabled = false;
                        btnFromPlus.Enabled = false;
                        btnToMinus.Enabled = false;
                        btnToPlus.Enabled = false;
                        MessageBox.Show("WAVファイルが読み込まれました。", "情報");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"WAVファイルの読み込みに失敗しました: {ex.Message}", "エラー");
                    }
                }
            }
        }

        /// <summary>
        /// CSVファイルを読み込み
        /// </summary>
        private void BtnLoadCsv_Click(object sender, EventArgs e)
        {
            if (audioFileReader == null)
            {
                MessageBox.Show("先にWAVファイルを選択してください。", "エラー");
                return;
            }

            using (var ofd = new OpenFileDialog { Filter = "CSVファイル|*.csv" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        intervals.Clear();
                        lstTimestamps.Items.Clear();
                        lstTimestamps.ClearSelected();
                        double maxDuration = audioFileReader.TotalTime.TotalSeconds;
                        using (var reader = new StreamReader(ofd.FileName))
                        {
                            reader.ReadLine(); // ヘッダー読み飛ばし
                            while (!reader.EndOfStream)
                            {
                                var line = reader.ReadLine().Split(',');
                                if (line.Length == 2 && double.TryParse(line[0], out double from) && double.TryParse(line[1], out double to))
                                {
                                    if (from >= 0 && from < to && to <= maxDuration)
                                    {
                                        intervals.Add((from, to));
                                        lstTimestamps.Items.Add(FormatTimestamp(from, to));
                                    }
                                }
                            }
                        }
                        if (intervals.Count == 0)
                        {
                            MessageBox.Show("有効な区間がありませんでした。", "情報");
                        }
                        else
                        {
                            MessageBox.Show("CSVを読み込みました。", "情報");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"CSV読み込みに失敗しました: {ex.Message}", "エラー");
                    }
                }
            }
        }

        /// <summary>
        /// 音声の再生/停止
        /// </summary>
        private void BtnPlay_Click(object sender, EventArgs e)
        {
            if (waveOut == null || audioFileReader == null)
            {
                MessageBox.Show("WAVファイルを選択してください。", "エラー");
                return;
            }

            if (waveOut.PlaybackState == PlaybackState.Playing)
            {
                waveOut.Pause();
                timer.Stop();
            }
            else
            {
                isVerifyMode = false;
                audioFileReader.Position = 0;
                waveOut.Play();
                timer.Start();
            }
        }

        /// <summary>
        /// 検証再生
        /// </summary>
        private void BtnVerify_Click(object sender, EventArgs e)
        {
            if (waveOut == null || audioFileReader == null)
            {
                MessageBox.Show("WAVファイルを選択してください。", "エラー");
                return;
            }

            if (intervals.Count == 0)
            {
                MessageBox.Show("区間が記録されていません。", "エラー");
                return;
            }

            if (waveOut.PlaybackState == PlaybackState.Playing)
            {
                waveOut.Pause();
                timer.Stop();
                isVerifyMode = false;
                currentIntervalIndex = -1;
                lstTimestamps.ClearSelected(); // 再生終了時に選択解除
            }
            else
            {
                isVerifyMode = true;
                currentIntervalIndex = lstTimestamps.SelectedIndex >= 0 ? lstTimestamps.SelectedIndex : 0;
                PlayNextInterval();
                timer.Start();
            }
        }

        /// <summary>
        /// 次の区間を再生
        /// </summary>
        private void PlayNextInterval()
        {
            if (!isVerifyMode || currentIntervalIndex < 0 || currentIntervalIndex >= intervals.Count)
            {
                waveOut?.Stop();
                timer.Stop();
                isVerifyMode = false;
                currentIntervalIndex = -1;
                lstTimestamps.ClearSelected(); // 全区間終了時に選択解除
                return;
            }

            var (from, to) = intervals[currentIntervalIndex];
            audioFileReader.CurrentTime = TimeSpan.FromSeconds(from);
            waveOut.Play();
        }

        /// <summary>
        /// 再生終了時の処理
        /// </summary>
        private void WaveOut_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (isVerifyMode)
            {
                currentIntervalIndex++;
                PlayNextInterval();
            }
            else
            {
                timer.Stop();
                lblTimer.Text = "00:00.000";
                lstTimestamps.ClearSelected(); // 再生終了時に選択解除
            }
        }

        /// <summary>
        /// タイムカウンターを更新
        /// </summary>
        private void UpdateTimer()
        {
            if (waveOut == null || audioFileReader == null || waveOut.PlaybackState != PlaybackState.Playing)
                return;

            double currentTime = audioFileReader.CurrentTime.TotalSeconds;
            lblTimer.Text = FormatTimestamp(currentTime);

            if (isVerifyMode && currentIntervalIndex >= 0 && currentIntervalIndex < intervals.Count)
            {
                var (_, to) = intervals[currentIntervalIndex];
                if (currentTime >= to)
                {
                    waveOut.Pause();
                    currentIntervalIndex++;
                    PlayNextInterval();
                }
                // 現在再生中の区間をアクティブに
                lstTimestamps.SelectedIndex = currentIntervalIndex;
                lstTimestamps.TopIndex = Math.Max(0, currentIntervalIndex - lstTimestamps.Height / lstTimestamps.ItemHeight + 1); // スクロール調整
            }
        }

        /// <summary>
        /// 2人目マーク開始（長押し開始）
        /// </summary>
        private void BtnMark_MouseDown(object sender, MouseEventArgs e)
        {
            if (waveOut == null || waveOut.PlaybackState != PlaybackState.Playing)
            {
                MessageBox.Show("音声を再生中にマークしてください。", "エラー");
                return;
            }
            markStart = audioFileReader.CurrentTime.TotalSeconds;
        }

        /// <summary>
        /// 2人目マーク終了（長押し終了）
        /// </summary>
        private void BtnMark_MouseUp(object sender, MouseEventArgs e)
        {
            if (markStart == null || waveOut == null || waveOut.PlaybackState != PlaybackState.Playing)
                return;

            double markEnd = audioFileReader.CurrentTime.TotalSeconds;
            if (markEnd <= markStart.Value)
            {
                markStart = null;
                return;
            }

            intervals.Add((markStart.Value, markEnd));
            string timestamp = FormatTimestamp(markStart.Value, markEnd);
            lstTimestamps.Items.Add(timestamp);
            lstTimestamps.TopIndex = lstTimestamps.Items.Count - 1;
            markStart = null;
        }

        /// <summary>
        /// タイムスタンプを mm:ss.ttt 形式にフォーマット（単一時間用）
        /// </summary>
        private string FormatTimestamp(double time)
        {
            TimeSpan span = TimeSpan.FromSeconds(time);
            return $"{span.Minutes:D2}:{span.Seconds:D2}.{span.Milliseconds:D3}";
        }

        /// <summary>
        /// タイムスタンプを mm:ss.ttt - mm:ss.ttt 形式にフォーマット（区間用）
        /// </summary>
        private string FormatTimestamp(double from, double to)
        {
            TimeSpan fromSpan = TimeSpan.FromSeconds(from);
            TimeSpan toSpan = TimeSpan.FromSeconds(to);
            return $"{fromSpan.Minutes:D2}:{fromSpan.Seconds:D2}.{fromSpan.Milliseconds:D3} - " +
                   $"{toSpan.Minutes:D2}:{toSpan.Seconds:D2}.{toSpan.Milliseconds:D3}";
        }

        /// <summary>
        /// 区間リストをCSVに保存
        /// </summary>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (intervals.Count == 0)
            {
                MessageBox.Show("記録された区間がありません。", "エラー");
                return;
            }

            using (var sfd = new SaveFileDialog { Filter = "CSVファイル|*.csv", FileName = "timestamps.csv" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var writer = new StreamWriter(sfd.FileName))
                        {
                            writer.WriteLine("from,to");
                            foreach (var (from, to) in intervals)
                            {
                                writer.WriteLine($"{from:F3},{to:F3}");
                            }
                        }
                        MessageBox.Show("CSVに保存しました。", "情報");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"CSV保存に失敗しました: {ex.Message}", "エラー");
                    }
                }
            }
        }

        /// <summary>
        /// 音声リソースの解放
        /// </summary>
        private void CleanupAudio()
        {
            waveOut?.Stop();
            waveOut?.Dispose();
            audioFileReader?.Dispose();
            waveOut = null;
            audioFileReader = null;
            timer?.Stop();
        }
    }
}