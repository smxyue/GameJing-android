using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using Android.Widget;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
using AlertDialog = Android.App.AlertDialog;
using static Android.Views.View;
using Android.Graphics;
using Android.Content;

namespace GameJing_android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true,Icon ="@drawable/ic_launcher")]
    public class MainActivity : AppCompatActivity
    {
        GameJing game = new GameJing(15);
        
        private AlertDialog _dialog;
        ImageView imgGame;
        float GridSize = 1;
        int currentRow = -1;
        int currentCol = -1;
        int imgSize = 1;
        TextView txtMsg = null;
        TextView txtTip = null;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            imgGame = (ImageView)FindViewById(Resource.Id.imageViewGame);
            imgGame.Touch += onGameTuched;

            txtMsg = (TextView)FindViewById(Resource.Id.textViewMsg);
            txtTip = (TextView)FindViewById(Resource.Id.textViewTip);
            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;
        }
        public override void OnWindowFocusChanged(bool hasFocus)
        {
            if (hasFocus)
            {
                Android.Util.DisplayMetrics dm = Resources.DisplayMetrics;
                imgSize = dm.WidthPixels -20;
                GridSize = imgSize / 3;
                imgGame.SetMaxHeight(imgSize);
                imgGame.SetMaxWidth(imgSize);
                showGame();
            }
            base.OnWindowFocusChanged(hasFocus);
            
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            switch(id)
            {
                case Resource.Id.menu_14:
                    game.Target = 14;
                    game.reset();
                    showGame();
                    break;
                case Resource.Id.menu_15:
                    game.Target = 15;
                    game.reset();
                    showGame();
                    break;
                case Resource.Id.menu_16:
                    game.Target = 16;
                    game.reset();
                    showGame();
                    break;
                case Resource.Id.menu_new:
                    game.reset();
                    showGame();
                    break;
                case Resource.Id.menu_about:
                    AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                    dialog.SetTitle("关于作者");
                    View dialoglayout = LayoutInflater.Inflate(Resource.Layout.layout1, null);
                    dialog.SetView(dialoglayout);
                    dialog.SetCancelable(true);
                    dialog.SetPositiveButton("OK", (s, e) =>
                    {
                    });
                    dialog.Show();
                    break;
                case Resource.Id.menu_setaifirst:
                    txtTip.Text = "";
                    game.reset();
                    game.AIGo();
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "穿别人的鞋，走自己的路，让别人无路可走！", Snackbar.LengthLong)
                .SetAction("Action", (View.IOnClickListener)null).Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        private int GetGridIndex(int row, int col)
        {
            return row * 3 + col;
        }
        private int GetGrid(float x)
        {
            int res = (int)x % (int)GridSize;
            if (res > 20)
            {
                return (int)(x / GridSize);
            }
            else
            {
                return -1;
            }
        }
        private void onGameTuched(object sender, View.TouchEventArgs e)
        {
            if (game.gameOver)
                return;

            if (e.Event.Action == MotionEventActions.Up)
            {
                float startx, starty;
                startx = e.Event.GetX();
                starty = e.Event.GetY();
                currentRow = GetGrid(starty);
                currentCol = GetGrid(startx);
                if (currentCol >= 0 && currentCol < 3 && currentRow >= 0 && currentRow < 3)
                {
                    int index = GetGridIndex(currentRow, currentCol);
                    if (game.GameGrid[index, 0] == 0)
                    {
                        AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                        View dialogView = LayoutInflater.From(this).Inflate(Resource.Layout.userinput, null);
                        Button btOk = (Button)dialogView.FindViewById(Resource.Id.buttonOk);
                        Button btCancle = (Button)dialogView.FindViewById(Resource.Id.buttonCancle);
                        EditText editNum = (EditText)dialogView.FindViewById(Resource.Id.editTextNum);
                        dialog.SetTitle("请输入你想填写的数字：");
                        dialog.SetView(dialogView);
                        btCancle.Click += (s, arg) =>
                         {
                             _dialog.Dismiss();
                         };
                        btOk.Click += (s, arg) =>
                        {
                            string str = editNum.Text.Trim();
                            if (str.Length > 0)
                            {
                                int val = int.Parse(str);
                                if (val > 0 && val < 10)
                                {
                                    if (game.IsUsed(val))
                                    {
                                        Toast.MakeText(this, "数字已经被使用过了，请输入一个没有被使用过的！", ToastLength.Long).Show();
                                    }
                                    else
                                    {
                                        game.setGrid(currentRow, currentCol, val, 0);
                                        if (game.checkWon(currentRow,currentCol))
                                        {
                                            SaveGame(true);
                                            gameWon("真聪明，你赢了！");
                                        }
                                        else
                                        {
                                            game.AIGo();
                                            if (game.aiWon)
                                            {
                                                SaveGame(false);
                                                gameWon("哈哈，我赢了，你怎么连个CPU都不如？！");
                                            }
                                        }
                                        if (game.checkDeadlock())
                                        {
                                            Toast.MakeText(this, "出现僵局！", ToastLength.Long).Show();
                                            txtTip.Text = "当前是僵局！";
                                        }
                                        currentCol = -1;
                                        currentRow = -1;
                                        _dialog.Dismiss();
                                        showGame();
                                    }
                                }
                            }
                        };
                        _dialog = dialog.Show();
                    }
                }
                else
                {
                    Toast.MakeText(this, "不要在线上点击，尽量居中，请选择你想要的空格。", ToastLength.Short).Show();
                }
            }
        }
        
        
        private void showGame()
        {
 
            Bitmap bitmap = Bitmap.CreateBitmap(imgSize, imgSize, Bitmap.Config.Argb8888);
            Canvas canvas = new Canvas(bitmap);
            Paint paint = new Paint();
            paint.Color = Color.Brown;
            paint.StrokeWidth = 3;
            paint.SetStyle(Paint.Style.Stroke);
            for(int i=0;i<=3;i++)
            {
                canvas.DrawLine(i * GridSize, 0, i * GridSize, imgSize, paint);
                canvas.DrawLine(0, i * GridSize, imgSize, i * GridSize, paint);
            }
            paint.TextSize = (int)(GridSize * 0.8);
            paint.SetStyle(Paint.Style.Fill);
            for(int i=0;i<3;i++)
            {
                for(int j=0;j<3;j++)
                {
                    float x = j * GridSize + 20;
                    float y = (i+1) * GridSize - 20;
                    int index = GetGridIndex(i, j) ;
                    int val = game.getGridValue(index);
                    if ( val > 0 && val < 10)
                    {
                        if (game.GetGridOwner(index) == 0)
                        {
                            paint.Color = Color.Red;
                        }
                        else
                        {
                            paint.Color = Color.Green;
                        }
                        canvas.DrawText(val.ToString(), x, y, paint);
                    }
                }
            }
            imgGame.SetImageBitmap(bitmap);
            txtMsg.Text ="轮流填入数字，看谁先在行、列或对角线上先凑出和" + game.Target.ToString() + "来.\n\r"+ game.getOkSetText() + game.getNumUsedText();
            if (!game.gameOver)
            {
                txtTip.Text = "you vs app:" + GetGame(true).ToString() + ":" + GetGame(false).ToString();
            }
        }
        public void gameWon(string msg)
        {
            txtTip.Text = msg;
            AlertDialog.Builder dialog = new AlertDialog.Builder(this);
            dialog.SetTitle("游戏结束");
            dialog.SetMessage(msg);
            dialog.Show();
        }
        public void SaveGame(bool isUser)
        {
            String keyword;
            if (isUser)
            {
                keyword = "user";
            }
            else
            {
                keyword = "ai";
            }
            ISharedPreferences prefs = GetSharedPreferences(keyword, Android.Content.FileCreationMode.Private);
            ISharedPreferencesEditor editor = prefs.Edit();

            int count = prefs.GetInt(keyword, 0);
            count++;
            editor.PutInt(keyword, count);
            editor.Apply();
        }
        public int GetGame(bool isUser)
        {
            String keyword;
            if (isUser)
            {
                keyword = "user";
            }
            else
            {
                keyword = "ai";
            }
            ISharedPreferences prefs = GetSharedPreferences(keyword, Android.Content.FileCreationMode.Private);

            int count = prefs.GetInt(keyword, 0);
            return count;
        }
    }
}
