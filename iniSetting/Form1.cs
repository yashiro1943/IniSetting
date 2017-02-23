using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using Others;
using AlgorithmTool;

namespace inisetting
{
    public partial class Form1 : Form
    {
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern long WritePrivateProfileString(string section,
        string key, string val, string filePath);
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section,
        string key, string def, StringBuilder retVal,
        int size, string filePath);
              
        public string filename = "ini_Hello.ini"; //your file name

        Ini Main_Ini = new Ini();

        /** \brief AGV 系統狀態*/
        public static rtAGV_Control DeliverData;

        public Form1()
        {
            InitializeComponent();
            Main_Ini.Read_ini_Cfg1();
            DeliverData = Main_Ini.Read_ini_Cfg2(DeliverData);
            
            panel_Main.Visible = true;
            panel_Map_Configure.Visible = false;
            panel_AGV_Configure.Visible = false;

            panel_Main.Size = new System.Drawing.Size(1090, 682);
            panel_Main.Location = new Point(0, 0);
            panel_Map_Configure.Size = new System.Drawing.Size(1090, 682);
            panel_Map_Configure.Location = new Point(0, 0);
            panel_AGV_Configure.Size = new System.Drawing.Size(1090, 682);
            panel_AGV_Configure.Location = new Point(0, 0);
        }

        private void button_AGV_Configure_Click(object sender, EventArgs e)
        {
            textBoxl_Car_ID.Text = Main_Ini.CAR_ID.ToString();
            textBox__Car_Length.Text = Main_Ini.CAR_LENGTH.ToString();
            textBox_Car_Width_Wheel.Text = Main_Ini.CAR_WIDTH_WHEEL.ToString();

            textBox_Motor_Power_Params_P.Text = Main_Ini.PID_POWER_COE_KP.ToString();
            textBox_Theta_Offset_Params_P.Text = Main_Ini.PID_THETA_OFFSET_COE_KP.ToString();
            textBox_Motor_Angle_Params_P.Text = Main_Ini.PID_ANGLE_CAR_MOTOR_COE_KP.ToString();

            textBox_Motor_Power_Params_I.Text = Main_Ini.PID_POWER_COE_KI.ToString();
            textBox_Theta_Offset_Params_I.Text = Main_Ini.PID_THETA_OFFSET_COE_KI.ToString();
            textBox_Motor_Angle_Params_I.Text = Main_Ini.PID_ANGLE_CAR_MOTOR_COE_KI.ToString();

            textBox_Motor_Power_Params_D.Text = Main_Ini.PID_POWER_COE_KD.ToString();
            textBox_Theta_Offset_Params_D.Text = Main_Ini.PID_THETA_OFFSET_COE_KD.ToString();
            textBox_Motor_Angle_Params_D.Text = Main_Ini.PID_ANGLE_CAR_MOTOR_COE_KD.ToString();

            panel_Main.Visible = false;
            panel_Map_Configure.Visible = true;
            panel_AGV_Configure.Visible = false;
        }

        object[] obj_AGV;
        object[] obj_MAP;
        private void button_Map_Configure_Click(object sender, EventArgs e)
        {
            panel_Main.Visible = false;
            panel_AGV_Configure.Visible = true;
            panel_Map_Configure.Visible = false;

            dataGridView_AGV_Configure.Rows.Clear(); //清空資料表

            //區域個數
            textBox_Region_Number.Text = DeliverData.tIS_Cfg.tMapCfg.alRegionNum.ToString();

            for (int i = 0; i <  Convert.ToInt32(textBox_Region_Number.Text); i++)
            {
                obj_AGV = new object[1] { "Region" + (i + 1).ToString() };
                DataGridViewRow dgvr = new DataGridViewRow();
                dgvr.CreateCells(dataGridView_AGV_Configure, obj_AGV);
                //隱藏陣列所引於Tag中
                dgvr.Tag = i;
                dgvr.Height = 35;
                //if (i % 2 == 0) dgvr.DefaultCellStyle.BackColor = Color.LightSkyBlue;
                //else dgvr.DefaultCellStyle.BackColor = Color.LightBlue;
                dataGridView_AGV_Configure.Rows.Add(dgvr);              
            }
        }

        private void button_Map_Configure_Bark_Click(object sender, EventArgs e)
        {
            Main_Ini.CAR_ID = textBoxl_Car_ID.Text;
            Main_Ini.CAR_LENGTH = Convert.ToInt32(textBox__Car_Length.Text);
            Main_Ini.CAR_WIDTH_WHEEL = Convert.ToInt32(textBox_Car_Width_Wheel.Text);

            Main_Ini.PID_POWER_COE_KP = Convert.ToDouble(textBox_Motor_Power_Params_P.Text);
            Main_Ini.PID_THETA_OFFSET_COE_KP = Convert.ToDouble(textBox_Theta_Offset_Params_P.Text);
            Main_Ini.PID_ANGLE_CAR_MOTOR_COE_KP = Convert.ToDouble(textBox_Motor_Angle_Params_P.Text);

            Main_Ini.PID_POWER_COE_KI = Convert.ToDouble(textBox_Motor_Power_Params_I.Text);
            Main_Ini.PID_THETA_OFFSET_COE_KI = Convert.ToDouble(textBox_Theta_Offset_Params_I.Text);
            Main_Ini.PID_ANGLE_CAR_MOTOR_COE_KI = Convert.ToDouble(textBox_Motor_Angle_Params_I.Text);

            Main_Ini.PID_POWER_COE_KD = Convert.ToDouble(textBox_Motor_Power_Params_D.Text);
            Main_Ini.PID_THETA_OFFSET_COE_KD = Convert.ToDouble(textBox_Theta_Offset_Params_D.Text);
            Main_Ini.PID_ANGLE_CAR_MOTOR_COE_KD = Convert.ToDouble(textBox_Motor_Angle_Params_D.Text);

            panel_Map_Configure.Visible = false;
            panel_Main.Visible = true;
        }

        private void button_AGV_Configure_Bark_Click(object sender, EventArgs e)
        {
            if (tabControl_AGV_Configure.Visible == true)
            {
                tabControl_AGV_Configure.Visible = false;
                label_MAP_Region_Setting.Visible = false;
                button_Modify_Region_Number.Visible = true;
                label_Map_Configure.Visible = true;
                dataGridView_AGV_Configure.Visible = true;
            }
            else
            {
                panel_Main.Visible = true;
                panel_AGV_Configure.Visible = false;
            }
        }

        int RegionNum = 0;
        int MAP_None_Number = 0;
        int MAP_Block_Number = 0;
        int MAP_WareHouse_Number = 0;
        
        private void dataGridView_AGV_Configure_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                int MachineTag = 0;
                //取得列所隱藏的Tag
                try
                {
                    if (dataGridView_AGV_Configure.Rows[e.RowIndex].Tag != null) MachineTag = Convert.ToInt16(dataGridView_AGV_Configure.Rows[e.RowIndex].Tag);
                    RegionNum = MachineTag;
                    button_Modify_Region_Number.Visible = false;
                    label_Map_Configure.Visible = false;
                    dataGridView_AGV_Configure.Visible = false;

                    tabControl_AGV_Configure.Visible = true;
                    label_MAP_Region_Setting.Text = "MAP-Region " + (MachineTag + 1) + "  setting";
                    label_MAP_Region_Setting.Visible = true;

                    //None_List
                    dataGridView_MAP_None_List.Rows.Clear(); //清空資料表

                    //讀取點選的區域的節點個數
                    textBox_MAP_Node_Number.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lNodeNum.ToString();
                    for (int i = 0; i < Convert.ToInt32(textBox_MAP_Node_Number.Text); i++)
                    {
                        obj_MAP = new object[1] { "None " + (i + 1).ToString() };
                        DataGridViewRow dgvr = new DataGridViewRow();
                        dgvr.CreateCells(dataGridView_MAP_None_List, obj_MAP);
                        //隱藏陣列所引於Tag中
                        dgvr.Tag = i;
                        dgvr.Height = 35;
                        //if (i % 2 == 0) dgvr.DefaultCellStyle.BackColor = Color.LightSkyBlue;
                        //else dgvr.DefaultCellStyle.BackColor = Color.LightBlue;
                        dataGridView_MAP_None_List.Rows.Add(dgvr);
                    }
                           

                    //Block List
                    dataGridView_MAP_Block_List.Rows.Clear(); //清空資料表
                    textBox_MAP_Block_Number.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lBlockNum.ToString();
                    for (int i = 0; i < Convert.ToInt32(textBox_MAP_Block_Number.Text); i++)
                    {
                        obj_MAP = new object[1] { "Block " + (i + 1).ToString() };
                        DataGridViewRow dgvr = new DataGridViewRow();
                        dgvr.CreateCells(dataGridView_MAP_Block_List, obj_MAP);
                        //隱藏陣列所引於Tag中
                        dgvr.Tag = i;
                        dgvr.Height = 35;
                        //if (i % 2 == 0) dgvr.DefaultCellStyle.BackColor = Color.LightSkyBlue;
                        //else dgvr.DefaultCellStyle.BackColor = Color.LightBlue;
                        dataGridView_MAP_Block_List.Rows.Add(dgvr);
                    }


                    //WareHouse List
                    dataGridView_MAP_WareHouse_List.Rows.Clear(); //清空資料表
                    textBox_MAP_WareHouse_Number.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lWarehouseNum.ToString();
                    for (int i = 0; i < Convert.ToInt32(textBox_MAP_WareHouse_Number.Text); i++)
                    {
                        obj_MAP = new object[1] { "WareHouse " + (i + 1).ToString() };
                        DataGridViewRow dgvr = new DataGridViewRow();
                        dgvr.CreateCells(dataGridView_MAP_WareHouse_List, obj_MAP);
                        //隱藏陣列所引於Tag中
                        dgvr.Tag = i;
                        dgvr.Height = 35;
                        //if (i % 2 == 0) dgvr.DefaultCellStyle.BackColor = Color.LightSkyBlue;
                        //else dgvr.DefaultCellStyle.BackColor = Color.LightBlue;
                        dataGridView_MAP_WareHouse_List.Rows.Add(dgvr);
                    }

                    textBox_MAP_Node_Index.Text = MAP_None_Number.ToString();

                    textBox_MAP_Block_Index.Text = MAP_Block_Number.ToString();

                    textBox_MAP_WareHouse_NodeID.Text = RegionNum.ToString();

                    textBox_MAP_WareHouse_Index.Text = MAP_WareHouse_Number.ToString();

                    textBox_MAP_Node_X.Text = "";
                    textBox_MAP_Node_Y.Text = "";
                    textBox_MAP_Block_X.Text = "";
                    textBox_MAP_Block_Y.Text = "";
                    textBox_MAP_Block_Width.Text = "";
                    textBox_MAP_Block_Height.Text = "";
                    textBox_MAP_WareHouse_Height.Text = "";
                    textBox_MAP_WareHouse_Depth.Text = "";
                    textBox_MAP_WareHouse_X.Text = "";
                    textBox_MAP_WareHouse_Y.Text = "";
                    textBox_MAP_WareHouse_Direction.Text = "";



                    textBox_MAP_Node_X.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atNode[MAP_None_Number].tCoordinate.eX.ToString();

                    textBox_MAP_Node_Y.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atNode[MAP_None_Number].tCoordinate.eY.ToString();



                    textBox_MAP_Block_X.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg[MAP_Block_Number].tCoordinate.eX.ToString();

                    textBox_MAP_Block_Y.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg[MAP_Block_Number].tCoordinate.eY.ToString();

                    textBox_MAP_Block_Width.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg[MAP_Block_Number].eWidth.ToString();

                    textBox_MAP_Block_Height.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg[MAP_Block_Number].eHeight.ToString();



                    textBox_MAP_WareHouse_Height.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[MAP_WareHouse_Number].eHeight.ToString();

                    textBox_MAP_WareHouse_Depth.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[MAP_WareHouse_Number].DistanceDepth.ToString();

                    textBox_MAP_WareHouse_X.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[MAP_WareHouse_Number].tCoordinate.eX.ToString();

                    textBox_MAP_WareHouse_Y.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[MAP_WareHouse_Number].tCoordinate.eY.ToString();

                    textBox_MAP_WareHouse_Direction.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[MAP_WareHouse_Number].eDirection.ToString();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("錯誤行數：{0}", ex.ToString());
                }
            }
        }


        private void button_Modify_Region_Number_Click(object sender, EventArgs e)
        {
            int oldRegionNum;
            dataGridView_AGV_Configure.Rows.Clear(); //清空資料表
            for (int i = 0; i < Convert.ToInt32(textBox_Region_Number.Text); i++)
            {
                obj_AGV = new object[1] { "Region" + (i + 1).ToString() };
                DataGridViewRow dgvr = new DataGridViewRow();
                dgvr.CreateCells(dataGridView_AGV_Configure, obj_AGV);
                //隱藏陣列所引於Tag中
                dgvr.Tag = i;
                dgvr.Height = 35;
                //if (i % 2 == 0) dgvr.DefaultCellStyle.BackColor = Color.LightSkyBlue;
                //else dgvr.DefaultCellStyle.BackColor = Color.LightBlue;
                dataGridView_AGV_Configure.Rows.Add(dgvr);
            }
            
            oldRegionNum = DeliverData.tIS_Cfg.tMapCfg.alRegionNum;

            #region 自走車地圖舊資料儲存
            //點增加行數
            int[] array_NodeNum = new int[DeliverData.tIS_Cfg.tMapCfg.alRegionNum];
            double[,] array_Node_X_All = new double[DeliverData.tIS_Cfg.tMapCfg.alRegionNum, DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[0].lNodeNum];
            double[,] array_Node_Y_All = new double[DeliverData.tIS_Cfg.tMapCfg.alRegionNum, DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[0].lNodeNum];

            //先記綠之前檔案
            for (int i = 0; i < DeliverData.tIS_Cfg.tMapCfg.alRegionNum; i++)
            {
                array_NodeNum[i] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].lNodeNum;

                for (int j = 0; j < DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].lNodeNum; j++)
                {
                    array_Node_X_All[i, j] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atNode[j].tCoordinate.eX;
                    array_Node_Y_All[i, j] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atNode[j].tCoordinate.eY;
                }
            }
            #endregion

            #region 區域舊資料儲存
            int[] array_BlockNum = new int[DeliverData.tIS_Cfg.tMapCfg.alRegionNum];
            double[,] array_Block_X_All = new double[DeliverData.tIS_Cfg.tMapCfg.alRegionNum, DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[0].lBlockNum];
            double[,] array_Block_Y_All = new double[DeliverData.tIS_Cfg.tMapCfg.alRegionNum, DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[0].lBlockNum];
            double[,] array_Block_Width_All = new double[DeliverData.tIS_Cfg.tMapCfg.alRegionNum, DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[0].lBlockNum];
            double[,] array_Block_Height_All = new double[DeliverData.tIS_Cfg.tMapCfg.alRegionNum, DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[0].lBlockNum];

            //先記綠之前檔案
            for (int i = 0; i < DeliverData.tIS_Cfg.tMapCfg.alRegionNum; i++)
            {
                array_BlockNum[i] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].lBlockNum;

                for (int j = 0; j < DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].lBlockNum; j++)
                {
                    array_Block_X_All[i, j] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atBlockCfg[j].tCoordinate.eX;
                    array_Block_Y_All[i, j] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atBlockCfg[j].tCoordinate.eY;
                    array_Block_Width_All[i, j] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atBlockCfg[j].eWidth;
                    array_Block_Height_All[i, j] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atBlockCfg[j].eHeight;
                }
            }
            #endregion

            #region 貨倉舊資料儲存
            int[] array_WareHouseNum = new int[DeliverData.tIS_Cfg.tMapCfg.alRegionNum];
            double[,] array_WareHouse_Height_All = new double[DeliverData.tIS_Cfg.tMapCfg.alRegionNum, DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[0].lWarehouseNum];
            double[,] array_WareHouse_Depth_All = new double[DeliverData.tIS_Cfg.tMapCfg.alRegionNum, DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[0].lWarehouseNum];
            double[,] array_WareHouse_X_All = new double[DeliverData.tIS_Cfg.tMapCfg.alRegionNum, DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[0].lWarehouseNum];
            double[,] array_WareHouse_Y_All = new double[DeliverData.tIS_Cfg.tMapCfg.alRegionNum, DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[0].lWarehouseNum];
            double[,] array_WareHouse_Direction_All = new double[DeliverData.tIS_Cfg.tMapCfg.alRegionNum, DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[0].lWarehouseNum];


            //先記綠之前檔案
            for (int i = 0; i < DeliverData.tIS_Cfg.tMapCfg.alRegionNum; i++)
            {
                array_WareHouseNum[i] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].lWarehouseNum;

                for (int j = 0; j < DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].lWarehouseNum; j++)
                {
                    array_WareHouse_Height_All[i, j] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atWarehouseCfg[j].eHeight;
                    array_WareHouse_Depth_All[i, j] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atWarehouseCfg[j].DistanceDepth;
                    array_WareHouse_X_All[i, j] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atWarehouseCfg[j].tCoordinate.eX;
                    array_WareHouse_Y_All[i, j] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atWarehouseCfg[j].tCoordinate.eY;
                    array_WareHouse_Direction_All[i, j] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atWarehouseCfg[j].eDirection;
                }
            }


            #endregion

            DeliverData.tIS_Cfg.tMapCfg.alRegionNum = Convert.ToInt32(textBox_Region_Number.Text);

            DeliverData.tIS_Cfg.tMapCfg.atRegionCfg = new RegionOfMap[DeliverData.tIS_Cfg.tMapCfg.alRegionNum];
            
            if (DeliverData.tIS_Cfg.tMapCfg.alRegionNum < oldRegionNum)
            {
                oldRegionNum = DeliverData.tIS_Cfg.tMapCfg.alRegionNum;
            }
            //新增新的格式
            #region 自走車地圖新資料寫入
            for (int i = 0; i < oldRegionNum; i++)
            {
                DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].lNodeNum = array_NodeNum[i];

                DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atNode = new rtNode[DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].lNodeNum];
                //先記綠之前檔案
                for (int j = 0; j < array_NodeNum[i]; j++)
                {
                    DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atNode[j].tCoordinate.eX = array_Node_X_All[i, j];
                    DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atNode[j].tCoordinate.eY = array_Node_Y_All[i, j];
                }
            }

            #endregion

            #region 區域新資料寫入

            for (int i = 0; i < oldRegionNum; i++)
            {
                DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].lBlockNum = array_BlockNum[i];

                DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atBlockCfg = new rtBlockInfo[DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].lBlockNum];
                //先記綠之前檔案
                for (int j = 0; j < array_BlockNum[i]; j++)
                {
                    DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atBlockCfg[j].tCoordinate.eX = array_Block_X_All[i, j];
                    DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atBlockCfg[j].tCoordinate.eY = array_Block_Y_All[i, j];
                    DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atBlockCfg[j].eWidth = array_Block_Width_All[i, j];
                    DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atBlockCfg[j].eHeight = array_Block_Height_All[i, j];
                }
            }
            #endregion

            #region 貨倉新資料寫入

            for (int i = 0; i < oldRegionNum; i++)
            {
                DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].lWarehouseNum = array_WareHouseNum[i];

                DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atWarehouseCfg = new rtWarehouseInfo[DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].lWarehouseNum];
                //先記綠之前檔案
                for (int j = 0; j < array_WareHouseNum[i]; j++)
                {
                    DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atWarehouseCfg[j].eHeight = array_WareHouse_Height_All[i, j];
                    DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atWarehouseCfg[j].DistanceDepth = array_WareHouse_Depth_All[i, j];
                    DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atWarehouseCfg[j].tCoordinate.eX = array_WareHouse_X_All[i, j];
                    DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atWarehouseCfg[j].tCoordinate.eY = array_WareHouse_Y_All[i, j];
                    DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atWarehouseCfg[j].eDirection = array_WareHouse_Direction_All[i, j];
                    
                }
            }
            #endregion
        }

        private void button_MAP_Read_Current_Cfg_Click(object sender, EventArgs e)
        {
            
        }

        int MAP_Node_Delete = 0;
        int MAP_Block_Delete = 0;
        int MAP_WareHouse_Delete = 0;

        private void button_MAP_Node_Delete_Click(object sender, EventArgs e)
        {
            MAP_Node_Delete = 1;
        }
    
        private void dataGridView_MAP_None_List_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                int MachineTag = 0;
                //取得列所隱藏的Tag
                try
                {
                    if (dataGridView_MAP_None_List.Rows[e.RowIndex].Tag != null) MachineTag = Convert.ToInt16(dataGridView_MAP_None_List.Rows[e.RowIndex].Tag);
                    MAP_None_Number = MachineTag;

                    if (MAP_Node_Delete == 0)
                    {
                        //點選要做的事
                        //DeliverData.tAGV_Cfg.tMapCfg.atNodeLocal[MachineTag].ToString();

                        //textBox_MAP_Node_Index.Text = DeliverData.tAGV_Cfg.tMapCfg.atNodeLocal[RegionNum][MachineTag].tNodeId.lRegion.ToString();

                        textBox_MAP_Node_Index.Text = MachineTag.ToString();

                        textBox_MAP_Node_X.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atNode[MachineTag].tCoordinate.eX.ToString();

                        textBox_MAP_Node_Y.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atNode[MachineTag].tCoordinate.eY.ToString();

                    }
                    else if (MAP_Node_Delete == 1)
                    {
                        //點刪除要做的事
                        MAP_Node_Delete = 0;
                        textBox_MAP_Node_Number.Text = (Convert.ToInt32(textBox_MAP_Node_Number.Text) - 1).ToString();
                        int delete_Number = 0;
                        double[] array_Node_X = new double[DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lNodeNum];
                        double[] array_Node_Y = new double[DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lNodeNum];

                        for (int j = 0; j < DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lNodeNum; j++)
                        {
                            if (j == MachineTag)
                            {
                                //不做任何事情;
                            }
                            else
                            {
                                array_Node_X[delete_Number] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atNode[j].tCoordinate.eX;
                                array_Node_Y[delete_Number] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atNode[j].tCoordinate.eY;
                                delete_Number++;
                            }
                        }
                        DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lNodeNum = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lNodeNum - 1;

                        DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atNode = new rtNode[DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lNodeNum];

                        for (int j = 0; j < DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lNodeNum; j++)
                        {
                            DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atNode[j].tCoordinate.eX = array_Node_X[j];
                            DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atNode[j].tCoordinate.eY = array_Node_Y[j];
                        }

                        dataGridView_MAP_None_List.Rows.Clear(); //清空資料表

                        for (int i = 0; i < Convert.ToInt32(textBox_MAP_Node_Number.Text); i++)
                        {
                            //dataGridView_MAP_Node_None_List.Rows.RemoveAt(MachineTag);
                            obj_MAP = new object[1] { "None" + (i + 1).ToString() };
                            DataGridViewRow dgvr = new DataGridViewRow();
                            dgvr.CreateCells(dataGridView_MAP_None_List, obj_MAP);
                            //隱藏陣列所引於Tag中
                            dgvr.Tag = i;
                            dgvr.Height = 35;
                            //if (i % 2 == 0) dgvr.DefaultCellStyle.BackColor = Color.LightSkyBlue;
                            //else dgvr.DefaultCellStyle.BackColor = Color.LightBlue;
                            dataGridView_MAP_None_List.Rows.Add(dgvr);
                        }
                        textBox_MAP_Node_Index.Text = "0";

                        textBox_MAP_Node_X.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atNode[0].tCoordinate.eX.ToString();

                        textBox_MAP_Node_Y.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atNode[0].tCoordinate.eY.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("錯誤行數：{0}", ex.ToString());
                }
            }
        }

        private void button_MAP_Node_Insert_Click(object sender, EventArgs e)
        {
            //點增加行數
            textBox_MAP_Node_Number.Text = (Convert.ToInt32(textBox_MAP_Node_Number.Text) + 1).ToString();

            double[] array_Node_X = new double[DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lNodeNum];
            double[] array_Node_Y = new double[DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lNodeNum];

            for (int j = 0; j < DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lNodeNum; j++)
            {
                array_Node_X[j] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atNode[j].tCoordinate.eX;
                array_Node_Y[j] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atNode[j].tCoordinate.eY;
            }
            DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lNodeNum = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lNodeNum + 1;

            DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atNode = new rtNode[DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lNodeNum];

            for (int j = 0; j < (DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lNodeNum - 1); j++)
            {
                DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atNode[j].tCoordinate.eX = array_Node_X[j];
                DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atNode[j].tCoordinate.eY = array_Node_Y[j];
            }

            dataGridView_MAP_None_List.Rows.Clear(); //清空資料表

            for (int i = 0; i < Convert.ToInt32(textBox_MAP_Node_Number.Text); i++)
            {
                obj_MAP = new object[1] { "None" + (i + 1).ToString() };
                DataGridViewRow dgvr = new DataGridViewRow();
                dgvr.CreateCells(dataGridView_MAP_None_List, obj_MAP);
                //隱藏陣列所引於Tag中
                dgvr.Tag = i;
                dgvr.Height = 35;
                //if (i % 2 == 0) dgvr.DefaultCellStyle.BackColor = Color.LightSkyBlue;
                //else dgvr.DefaultCellStyle.BackColor = Color.LightBlue;
                dataGridView_MAP_None_List.Rows.Add(dgvr);
            }
        }
        
        private void dataGridView_MAP_Block_List_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                int MachineTag = 0;
                //取得列所隱藏的Tag
                try
                {
                    if (dataGridView_MAP_Block_List.Rows[e.RowIndex].Tag != null) MachineTag = Convert.ToInt16(dataGridView_MAP_Block_List.Rows[e.RowIndex].Tag);
                    MAP_Block_Number = MachineTag;

                    if (MAP_Block_Delete == 0)
                    {
                        textBox_MAP_Block_Index.Text = MachineTag.ToString();

                        textBox_MAP_Block_X.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg[MachineTag].tCoordinate.eX.ToString();

                        textBox_MAP_Block_Y.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg[MachineTag].tCoordinate.eY.ToString();

                        textBox_MAP_Block_Width.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg[MachineTag].eWidth.ToString();

                        textBox_MAP_Block_Height.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg[MachineTag].eHeight.ToString();
                        //點選要做的事
                    }
                    else if (MAP_Block_Delete == 1)
                    {
                        //點刪除要做的事
                        MAP_Block_Delete = 0;
                        textBox_MAP_Block_Number.Text = (Convert.ToInt32(textBox_MAP_Block_Number.Text) - 1).ToString();

                        int delete_Number = 0;
                        double[] array_Block_X = new double[DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lBlockNum];
                        double[] array_Block_Y = new double[DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lBlockNum];
                        double[] array_Block_Width = new double[DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lBlockNum];
                        double[] array_Block_Height = new double[DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lBlockNum];

                        for (int j = 0; j < DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lBlockNum; j++)
                        {
                            if (j == MachineTag)
                            {
                                //不做任何事情;
                            }
                            else
                            {
                                array_Block_X[delete_Number] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg[j].tCoordinate.eX;
                                array_Block_Y[delete_Number] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg[j].tCoordinate.eY;
                                array_Block_Width[delete_Number] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg[j].eWidth;
                                array_Block_Height[delete_Number] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg[j].eHeight;
                                delete_Number++;
                            }
                        }
                        DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lBlockNum = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lBlockNum - 1;

                        DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg = new rtBlockInfo[DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lBlockNum];

                        for (int j = 0; j < DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lBlockNum; j++)
                        {
                            DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg[j].tCoordinate.eX = array_Block_X[j];
                            DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg[j].tCoordinate.eY = array_Block_Y[j];
                            DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg[j].eWidth = array_Block_Width[j];
                            DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg[j].eHeight = array_Block_Height[j];
                        }

                        dataGridView_MAP_Block_List.Rows.Clear(); //清空資料表

                        for (int i = 0; i < Convert.ToInt32(textBox_MAP_Block_Number.Text); i++)
                        {
                            //dataGridView_MAP_Node_None_List.Rows.RemoveAt(MachineTag);
                            obj_MAP = new object[1] { "Block" + (i + 1).ToString() };
                            DataGridViewRow dgvr = new DataGridViewRow();
                            dgvr.CreateCells(dataGridView_MAP_Block_List, obj_MAP);
                            //隱藏陣列所引於Tag中
                            dgvr.Tag = i;
                            dgvr.Height = 35;
                            //if (i % 2 == 0) dgvr.DefaultCellStyle.BackColor = Color.LightSkyBlue;
                            //else dgvr.DefaultCellStyle.BackColor = Color.LightBlue;
                            dataGridView_MAP_Block_List.Rows.Add(dgvr);
                        }
                        textBox_MAP_Block_Index.Text = "0";
                        textBox_MAP_Block_X.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg[0].tCoordinate.eX.ToString();
                        textBox_MAP_Block_Y.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg[0].tCoordinate.eY.ToString();
                        textBox_MAP_Block_Width.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg[0].eWidth.ToString();
                        textBox_MAP_Block_Height.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg[0].eHeight.ToString();                    
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("錯誤行數：{0}", ex.ToString());

                }
            }
        }

        private void button_MAP_Block_Delete_Click(object sender, EventArgs e)
        {
            MAP_Block_Delete = 1;
        }

        private void button_MAP_Block_Insert_Click(object sender, EventArgs e)
        {
            //點增加行數
            textBox_MAP_Block_Number.Text = (Convert.ToInt32(textBox_MAP_Block_Number.Text) + 1).ToString();

            double[] array_Block_X = new double[DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lBlockNum];
            double[] array_Block_Y = new double[DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lBlockNum];
            double[] array_Block_Width = new double[DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lBlockNum];
            double[] array_Block_Height = new double[DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lBlockNum];

            for (int j = 0; j < DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lBlockNum; j++)
            {
                array_Block_X[j] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg[j].tCoordinate.eX;
                array_Block_Y[j] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg[j].tCoordinate.eY;
                array_Block_Width[j] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg[j].eWidth;
                array_Block_Height[j] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg[j].eHeight;
            }
            DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lBlockNum = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lBlockNum + 1;

            DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg = new rtBlockInfo[DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lBlockNum];

            for (int j = 0; j < (DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lBlockNum - 1); j++)
            {
                DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg[j].tCoordinate.eX = array_Block_X[j];
                DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg[j].tCoordinate.eY = array_Block_Y[j];
                DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg[j].eWidth = array_Block_Width[j];
                DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg[j].eHeight = array_Block_Height[j];
            }

            dataGridView_MAP_Block_List.Rows.Clear(); //清空資料表

            for (int i = 0; i < Convert.ToInt32(textBox_MAP_Block_Number.Text); i++)
            {
                //dataGridView_MAP_Node_None_List.Rows.RemoveAt(MachineTag);
                obj_MAP = new object[1] { "Block" + (i + 1).ToString() };
                DataGridViewRow dgvr = new DataGridViewRow();
                dgvr.CreateCells(dataGridView_MAP_Block_List, obj_MAP);
                //隱藏陣列所引於Tag中
                dgvr.Tag = i;
                dgvr.Height = 35;
                //if (i % 2 == 0) dgvr.DefaultCellStyle.BackColor = Color.LightSkyBlue;
                //else dgvr.DefaultCellStyle.BackColor = Color.LightBlue;
                dataGridView_MAP_Block_List.Rows.Add(dgvr);
            }
        }

        private void dataGridView_MAP_WareHouse_List_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                int MachineTag = 0;
                //取得列所隱藏的Tag
                try
                {
                    if (dataGridView_MAP_WareHouse_List.Rows[e.RowIndex].Tag != null) MachineTag = Convert.ToInt16(dataGridView_MAP_WareHouse_List.Rows[e.RowIndex].Tag);
                    MAP_WareHouse_Number = MachineTag;

                    if (MAP_WareHouse_Delete == 0)
                    {
                        textBox_MAP_WareHouse_NodeID.Text = RegionNum.ToString();

                        textBox_MAP_WareHouse_Index.Text = MachineTag.ToString();

                        textBox_MAP_WareHouse_Height.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[MachineTag].eHeight.ToString();

                        textBox_MAP_WareHouse_Depth.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[MachineTag].DistanceDepth.ToString();

                        textBox_MAP_WareHouse_X.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[MachineTag].tCoordinate.eX.ToString();

                        textBox_MAP_WareHouse_Y.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[MachineTag].tCoordinate.eY.ToString();

                        textBox_MAP_WareHouse_Direction.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[MachineTag].eDirection.ToString();
                        //點選要做的事
                    }
                    else if (MAP_WareHouse_Delete == 1)
                    {
                        //點刪除要做的事
                        MAP_WareHouse_Delete = 0;
                        textBox_MAP_WareHouse_Number.Text = (Convert.ToInt32(textBox_MAP_WareHouse_Number.Text) - 1).ToString();

                        int delete_Number = 0;

                        //double[] array_WareHouse_Index = new double[DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lWarehouseNum];
                        //double[] array_WareHouse_NodeID = new double[DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lWarehouseNum];
                        double[] array_WareHouse_Height = new double[DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lWarehouseNum];
                        double[] array_WareHouse_Depth = new double[DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lWarehouseNum];
                        double[] array_WareHouse_X = new double[DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lWarehouseNum];
                        double[] array_WareHouse_Y = new double[DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lWarehouseNum];
                        double[] array_WareHouse_Direction = new double[DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lWarehouseNum];

                        for (int j = 0; j < DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lWarehouseNum; j++)
                        {
                            if (j == MachineTag)
                            {
                                //不做任何事情;
                            }
                            else
                            {
                                array_WareHouse_Height[delete_Number] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[j].eHeight;
                                array_WareHouse_Depth[delete_Number] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[j].DistanceDepth;
                                array_WareHouse_X[delete_Number] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[j].tCoordinate.eX;
                                array_WareHouse_Y[delete_Number] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[j].tCoordinate.eY;
                                array_WareHouse_Direction[delete_Number] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[j].eDirection;

                                delete_Number++;
                            }
                        }
                        DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lWarehouseNum = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lWarehouseNum - 1;

                        DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg = new rtWarehouseInfo[DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lWarehouseNum];

                        for (int j = 0; j < DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lWarehouseNum; j++)
                        {
                            DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[j].eHeight = array_WareHouse_Height[j];
                            DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[j].DistanceDepth = array_WareHouse_Depth[j];
                            DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[j].tCoordinate.eX = array_WareHouse_X[j];
                            DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[j].tCoordinate.eY = array_WareHouse_Y[j];
                            DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[j].eDirection = array_WareHouse_Direction[j];
                        }

                        dataGridView_MAP_WareHouse_List.Rows.Clear(); //清空資料表

                        for (int i = 0; i < Convert.ToInt32(textBox_MAP_WareHouse_Number.Text); i++)
                        {
                            //dataGridView_MAP_Node_None_List.Rows.RemoveAt(MachineTag);
                            obj_MAP = new object[1] { "WareHouse" + (i + 1).ToString() };
                            DataGridViewRow dgvr = new DataGridViewRow();
                            dgvr.CreateCells(dataGridView_MAP_WareHouse_List, obj_MAP);
                            //隱藏陣列所引於Tag中
                            dgvr.Tag = i;
                            dgvr.Height = 35;
                            //if (i % 2 == 0) dgvr.DefaultCellStyle.BackColor = Color.LightSkyBlue;
                            //else dgvr.DefaultCellStyle.BackColor = Color.LightBlue;
                            dataGridView_MAP_WareHouse_List.Rows.Add(dgvr);
                        }

                        textBox_MAP_WareHouse_Index.Text = "0";
                        textBox_MAP_WareHouse_NodeID.Text = RegionNum.ToString(); ;
                        textBox_MAP_WareHouse_Height.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[0].eHeight.ToString();
                        textBox_MAP_WareHouse_Depth.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[0].DistanceDepth.ToString();
                        textBox_MAP_WareHouse_X.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[0].tCoordinate.eX.ToString();
                        textBox_MAP_WareHouse_Y.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[0].tCoordinate.eX.ToString();
                        textBox_MAP_WareHouse_Direction.Text = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[0].eDirection.ToString();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("錯誤行數：{0}", ex.ToString());
                }
            }
        }

        private void button_MAP_WareHouse_Delete_Click(object sender, EventArgs e)
        {
            MAP_WareHouse_Delete = 1;
        }

        private void button_MAP_WareHouse_Insert_Click(object sender, EventArgs e)
        {
            textBox_MAP_WareHouse_Number.Text = (Convert.ToInt32(textBox_MAP_WareHouse_Number.Text) + 1).ToString();
            double[] array_WareHouse_Height = new double[DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lWarehouseNum];
            double[] array_WareHouse_Depth = new double[DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lWarehouseNum];
            double[] array_WareHouse_X = new double[DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lWarehouseNum];
            double[] array_WareHouse_Y = new double[DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lWarehouseNum];
            double[] array_WareHouse_Direction = new double[DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lWarehouseNum];

            for (int j = 0; j < DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lWarehouseNum; j++)
            {
                array_WareHouse_Height[j] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[j].eHeight;
                array_WareHouse_Depth[j] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[j].DistanceDepth;
                array_WareHouse_X[j] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[j].tCoordinate.eX;
                array_WareHouse_Y[j] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[j].tCoordinate.eY;
                array_WareHouse_Direction[j] = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[j].eDirection;
            }
            DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lWarehouseNum = DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lWarehouseNum + 1;

            DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg = new rtWarehouseInfo[DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lWarehouseNum];

            for (int j = 0; j < (DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].lWarehouseNum - 1); j++)
            {
                DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[j].eHeight = array_WareHouse_Height[j];
                DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[j].DistanceDepth = array_WareHouse_Depth[j];
                DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[j].tCoordinate.eX = array_WareHouse_X[j];
                DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[j].tCoordinate.eY = array_WareHouse_Y[j];
                DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[j].eDirection = array_WareHouse_Direction[j];
            }

            dataGridView_MAP_WareHouse_List.Rows.Clear(); //清空資料表

            for (int i = 0; i < Convert.ToInt32(textBox_MAP_WareHouse_Number.Text); i++)
            {
                //dataGridView_MAP_Node_None_List.Rows.RemoveAt(MachineTag);
                obj_MAP = new object[1] { "WareHouse" + (i + 1).ToString() };
                DataGridViewRow dgvr = new DataGridViewRow();
                dgvr.CreateCells(dataGridView_MAP_WareHouse_List, obj_MAP);
                //隱藏陣列所引於Tag中
                dgvr.Tag = i;
                dgvr.Height = 35;
                //if (i % 2 == 0) dgvr.DefaultCellStyle.BackColor = Color.LightSkyBlue;
                //else dgvr.DefaultCellStyle.BackColor = Color.LightBlue;
                dataGridView_MAP_WareHouse_List.Rows.Add(dgvr);

            }
        }

        private void button_MAP_Node_Save_Click(object sender, EventArgs e)
        {
            try
            {
                DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atNode[MAP_None_Number].tCoordinate.eX = Convert.ToInt32(textBox_MAP_Node_X.Text);
                DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atNode[MAP_None_Number].tCoordinate.eY = Convert.ToInt32(textBox_MAP_Node_Y.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("錯誤操作：尚未新增資料及數據");
            }
        }

        private void button_MAP_Block_Save_Click(object sender, EventArgs e)
        {
            try
            {
                DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg[MAP_Block_Number].tCoordinate.eX = Convert.ToInt32(textBox_MAP_Block_X.Text);
                DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg[MAP_Block_Number].tCoordinate.eY = Convert.ToInt32(textBox_MAP_Block_Y.Text);
                DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg[MAP_Block_Number].eWidth = Convert.ToInt32(textBox_MAP_Block_Width.Text);
                DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atBlockCfg[MAP_Block_Number].eHeight = Convert.ToInt32(textBox_MAP_Block_Height.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("錯誤操作：尚未新增資料及數據");
            }
        }

        private void button_MAP_WareHouse_Save_Click(object sender, EventArgs e)
        {
            try
            {
                DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[MAP_WareHouse_Number].eHeight = Convert.ToInt32(textBox_MAP_WareHouse_Height.Text);
                DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[MAP_WareHouse_Number].DistanceDepth = Convert.ToInt32(textBox_MAP_WareHouse_Depth.Text);
                DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[MAP_WareHouse_Number].tCoordinate.eX = Convert.ToInt32(textBox_MAP_WareHouse_X.Text);
                DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[MAP_WareHouse_Number].tCoordinate.eY = Convert.ToInt32(textBox_MAP_WareHouse_Y.Text);
                DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[RegionNum].atWarehouseCfg[MAP_WareHouse_Number].eDirection = Convert.ToInt32(textBox_MAP_WareHouse_Direction.Text);

            }
            catch (Exception)
            {
                MessageBox.Show("錯誤操作：尚未新增資料及數據");
            }
        }

        private void button_Save_Map_Configure_Click(object sender, EventArgs e)
        {
            WritePrivateProfileString("AGV MAP Cfg", "Region_Num", DeliverData.tIS_Cfg.tMapCfg.alRegionNum.ToString(), ".\\" + filename);

            for (int i = 0; i < DeliverData.tIS_Cfg.tMapCfg.alRegionNum; i++)
            {
                #region 自走車地圖設定儲存為ini
                WritePrivateProfileString("AGV MAP Cfg", "Node_Num_Local_" + i.ToString(), DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].lNodeNum.ToString(), ".\\" + filename);
                for (int j = 0; j < DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].lNodeNum; j++)
                {
                    WritePrivateProfileString("AGV MAP Cfg", "Region_Index_Id_" + i.ToString() + "_" + j.ToString(), j.ToString(), ".\\" + filename);
       
                    try
                    {
                        WritePrivateProfileString("AGV MAP Cfg", "Region_Coordinate_X_" + i.ToString() + "_" + j.ToString(), DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atNode[j].tCoordinate.eX.ToString(), ".\\" + filename);
                    }
                    catch (Exception)
                    {
                        WritePrivateProfileString("AGV MAP Cfg", "Region_Coordinate_X_" + i.ToString() + "_" + j.ToString(), "0", ".\\" + filename);
                    }


                    try
                    {
                        WritePrivateProfileString("AGV MAP Cfg", "Region_Coordinate_Y_" + i.ToString() + "_" + j.ToString(), DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atNode[j].tCoordinate.eY.ToString(), ".\\" + filename);
                    }
                    catch (Exception)
                    {
                        WritePrivateProfileString("AGV MAP Cfg", "Region_Coordinate_Y_" + i.ToString() + "_" + j.ToString(), "0", ".\\" + filename);
                    }    
                }
                #endregion

                #region 區域資訊設定儲存為ini
                WritePrivateProfileString("Block Info", "Block_Number_" + i.ToString(), DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].lBlockNum.ToString(), ".\\" + filename);
                for (int j = 0; j < DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].lBlockNum; j++)
                {
                    WritePrivateProfileString("Block Info", "Block_Index_Id_" + i.ToString() + "_" + j.ToString(), j.ToString(), ".\\" + filename);

                    try
                    {
                        WritePrivateProfileString("Block Info", "Block_X_" + i.ToString() + "_" + j.ToString(), DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atBlockCfg[j].tCoordinate.eX.ToString(), ".\\" + filename);
                    }
                    catch (Exception)
                    {
                        WritePrivateProfileString("Block Info", "Block_X_" + i.ToString() + "_" + j.ToString(), "0", ".\\" + filename);
                    }

                    try
                    {
                        WritePrivateProfileString("Block Info", "Block_Y_" + i.ToString() + "_" + j.ToString(), DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atBlockCfg[j].tCoordinate.eY.ToString(), ".\\" + filename);
                    }
                    catch (Exception)
                    {
                        WritePrivateProfileString("Block Info", "Block_Y_" + i.ToString() + "_" + j.ToString(), "0", ".\\" + filename);
                    }

                    try
                    {
                        WritePrivateProfileString("Block Info", "Block_Width_" + i.ToString() + "_" + j.ToString(), DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atBlockCfg[j].eWidth.ToString(), ".\\" + filename);
                    }
                    catch (Exception)
                    {
                        WritePrivateProfileString("Block Info", "Block_Width_" + i.ToString() + "_" + j.ToString(), "0", ".\\" + filename);
                    }

                    try
                    {
                        WritePrivateProfileString("Block Info", "Block_Height_" + i.ToString() + "_" + j.ToString(), DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atBlockCfg[j].eHeight.ToString(), ".\\" + filename);
                    }
                    catch (Exception)
                    {
                        WritePrivateProfileString("Block Info", "Block_Height_" + i.ToString() + "_" + j.ToString(), "0", ".\\" + filename);
                    }
                }
                #endregion

                #region 貨倉資訊設定儲存為ini
                WritePrivateProfileString("Warehousing Info", "Container_Number_" + i.ToString(), DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].lWarehouseNum.ToString(), ".\\" + filename);
                for (int j = 0; j < DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].lWarehouseNum; j++)
                {
                    WritePrivateProfileString("Warehousing Info", "Container_Index_Id_" + i.ToString() + "_" + j.ToString(), j.ToString(), ".\\" + filename);

                    try
                    {
                        WritePrivateProfileString("Warehousing Info", "Container_Height_" + i.ToString() + "_" + j.ToString(), DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atWarehouseCfg[j].eHeight.ToString(), ".\\" + filename);
                    }
                    catch (Exception)
                    {
                        WritePrivateProfileString("Warehousing Info", "Container_Height_" + i.ToString() + "_" + j.ToString(), "0", ".\\" + filename);
                    }

                    try
                    {
                        WritePrivateProfileString("Warehousing Info", "Container_Depth_" + i.ToString() + "_" + j.ToString(), DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atWarehouseCfg[j].DistanceDepth.ToString(), ".\\" + filename);
                    }
                    catch (Exception)
                    {
                        WritePrivateProfileString("Warehousing Info", "Container_Depth_" + i.ToString() + "_" + j.ToString(), "0", ".\\" + filename);
                    }


                    try
                    {
                        WritePrivateProfileString("Warehousing Info", "Container_X_" + i.ToString() + "_" + j.ToString(), DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atWarehouseCfg[j].tCoordinate.eX.ToString(), ".\\" + filename);
                    }
                    catch (Exception)
                    {
                        WritePrivateProfileString("Warehousing Info", "Container_X_" + i.ToString() + "_" + j.ToString(), "0", ".\\" + filename);
                    }

                    try
                    {
                        WritePrivateProfileString("Warehousing Info", "Container_Y_" + i.ToString() + "_" + j.ToString(), DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atWarehouseCfg[j].tCoordinate.eY.ToString(), ".\\" + filename);
                    }
                    catch (Exception)
                    {
                        WritePrivateProfileString("Warehousing Info", "Container_Y_" + i.ToString() + "_" + j.ToString(), "0", ".\\" + filename);
                    }


                    try
                    {
                        WritePrivateProfileString("Warehousing Info", "Container_Direction_" + i.ToString() + "_" + j.ToString(), DeliverData.tIS_Cfg.tMapCfg.atRegionCfg[i].atWarehouseCfg[j].eDirection.ToString(), ".\\" + filename);
                    }
                    catch (Exception)
                    {
                        WritePrivateProfileString("Warehousing Info", "Container_Direction_" + i.ToString() + "_" + j.ToString(), "0", ".\\" + filename);
                    }
                }
                #endregion
            }
        }

        private void button_Save_AGV_Configure_Click(object sender, EventArgs e)
        {
            try
            {
                WritePrivateProfileString("Car Cfg", "CAR_ID", Main_Ini.CAR_ID.ToString(), ".\\" + filename);
            }
            catch (Exception)
            {
                WritePrivateProfileString("Car Cfg", "CAR_ID", "0", ".\\" + filename);
            }

            try
            {
                WritePrivateProfileString("Car Cfg", "CAR_LENGTH", Main_Ini.CAR_LENGTH.ToString(), ".\\" + filename);
            }
            catch (Exception)
            {
                WritePrivateProfileString("Car Cfg", "CAR_LENGTH", "0", ".\\" + filename);
            }

            try
            {
                WritePrivateProfileString("Car Cfg", "CAR_WIDTH_WHEEL", Main_Ini.CAR_WIDTH_WHEEL.ToString(), ".\\" + filename);
            }
            catch (Exception)
            {
                WritePrivateProfileString("Car Cfg", "CAR_WIDTH_WHEEL", "0", ".\\" + filename);
            }


            
            


            WritePrivateProfileString("Motor Control Cfg", "PID_POWER_COE_KP", Main_Ini.PID_POWER_COE_KP.ToString(), ".\\" + filename);
            WritePrivateProfileString("Motor Control Cfg", "PID_THETA_OFFSET_COE_KP", Main_Ini.PID_THETA_OFFSET_COE_KP.ToString(), ".\\" + filename);
            WritePrivateProfileString("Motor Control Cfg", "PID_ANGLE_CAR_MOTOR_COE_KP", Main_Ini.PID_ANGLE_CAR_MOTOR_COE_KP.ToString(), ".\\" + filename);


            WritePrivateProfileString("Motor Control Cfg", "PID_POWER_COE_KI", Main_Ini.PID_POWER_COE_KI.ToString(), ".\\" + filename);
            WritePrivateProfileString("Motor Control Cfg", "PID_THETA_OFFSET_COE_KI", Main_Ini.PID_THETA_OFFSET_COE_KI.ToString(), ".\\" + filename);
            WritePrivateProfileString("Motor Control Cfg", "PID_ANGLE_CAR_MOTOR_COE_KI", Main_Ini.PID_ANGLE_CAR_MOTOR_COE_KI.ToString(), ".\\" + filename);

            WritePrivateProfileString("Motor Control Cfg", "PID_POWER_COE_KD", Main_Ini.PID_POWER_COE_KD.ToString(), ".\\" + filename);
            WritePrivateProfileString("Motor Control Cfg", "PID_THETA_OFFSET_COE_KD", Main_Ini.PID_THETA_OFFSET_COE_KD.ToString(), ".\\" + filename);
            WritePrivateProfileString("Motor Control Cfg", "PID_ANGLE_CAR_MOTOR_COE_KD", Main_Ini.PID_ANGLE_CAR_MOTOR_COE_KD.ToString(), ".\\" + filename);
        }

        /*private void button1_Click(object sender, EventArgs e)
        {
            WritePrivateProfileString("section1", "var1", textBox1.Text, ".\\" + filename);
            WritePrivateProfileString("section2", "var1", textBox2.Text, ".\\" + filename);
            WritePrivateProfileString("section1", "var2", textBox3.Text, ".\\" + filename);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            int size = 3000;//temp file source size
            StringBuilder temp = new StringBuilder(size); //temp file source
            try
            {
                GetPrivateProfileString("section1", "var1", "", temp, size, ".\\" + filename);
                textBox1.Text = Convert.ToString(temp);
                GetPrivateProfileString("section2", "var1", "", temp, size, ".\\" + filename);
                textBox2.Text = Convert.ToString(temp);
                GetPrivateProfileString("section1", "var2", "", temp, size, ".\\" + filename);
                textBox3.Text = Convert.ToString(temp);
            }
            catch
            {

            }
       }*/
    }
}
