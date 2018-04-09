using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

public class GameConfigs {
	private static GameConfigs instance = null;

	public const string real_att_laptop = "real_att_laptop";
	public const string real_att_desktop = "real_att_desktop";
	public const string real_res_server = "real_res_server";
	public const string real_link_switch = "real_link_switch";
	public const string real_link_router = "real_link_router";
	public const string real_link_firewall = "real_link_firewall";
	public const string vir_att_shipleader = "vir_att_shipleader";
	public const string vir_att_shipgroup = "vir_att_shipgroup";
	public const string vir_res_asset = "vir_res_asset";
	public const string real_arbiter = "real_arbiter";
	public const string virtual_arbiter = "virtual_arbiter";

	public const string virtual_empty = "vir_empty";
	public const string real_empty = "real_empty";
    /// <summary>
    /// 2017.05.08
    /// To Change Add Dezzle Light on Arbiter to Show RoundNumber
    /// </summary>
    public const string virtual_dazzle = "virtual_dazzle";
	public const string behavior_attack_weak = "attack_weak";
	public const string behavior_attack_moderate = "attack_moderate";
	public const string behavior_attack_heavy = "attack_heavy";
	public const string behavior_attack_charge = "attack_charge";
	public const string behavior_enhance = "enhance";
	public const string behavior_gather = "gather";

	public const string NPC_Arbiter = "NPC.arbiter";
	public const string NPC_puzzle = "NPC.puzzle";

	public const string messageType_teams = "teams";
	public const string messageType_behavior = "behavior";
	public const string messageType_config = "config";
	public const string messageType_command = "command";

    // add gamer over scene
    public const string messageType_topScore = "topscoreboard";

	public bool show_npc = true;
	public float circle_1_angle = 15f;
	public float circle_2_angle = 30f;
	public float circle_3_angle = 0f;

	public float circle_1_arbitY = 15f;
	public float circle_2_arbitY = 15f;
	public float circle_3_arbitY = 15f;

	public float circle_1_radii = 15f;
	public float circle_2_radii = 35f;
	public float circle_3_radii = 50f;
	public int circle_1_min = 3;
	public int circle_1_max = 6;
	public int circle_2_min = 6;
	public int circle_2_max = 12;
	public int circle_3_max = 24;
	public float virtual_cam_rotationY = 10f;
	public float virtual_cam_rotationX = 1f;
	public float virtual_gravity = -2.2f;

	public float vir_unit_grid_x = 2.8f;
	public float vir_unit_grid_z = 2.8f;
	public float vir_unit_grid_y = 2.4f;
	public int vir_unit_usedepth_num = 8;
	public int vir_unit_arrange_depth = 2;
	public int vir_uint_start_num = 3;
	public int vir_uint_inc_step = 2;
	public float vir_unit_waveY_offset = -1f;
	public float vir_unit_waveZ = 1.3f;
	public float vir_leaderOffsetZ = -3f;
	public float vir_missile_speed = 10f;
	public float real_missile_speed = 10f;

	public int real_max_countX = 4;
    //change  floor  Scale   from 1 to  6
    public float real_floor_scale = 10.0f;
	public float real_unit_gap = 1.8f;
	public float real_cam_rotationY = 10f;
	public float real_cam_rotationX = 1f;
    public string logo = null;
    public bool scene_default_virtual = true ;

    private GameConfigs() {
		if (instance == null) {
			instance = this;
		}
	}

	public static GameConfigs Instance {
		get {
			if (instance == null) {
				instance = new GameConfigs();
			}
			return instance;
		}
        
	}

	public enum ColorType {
		red,
		yellow,
		blue,
		orange,
		purple,
		green
	}

	public enum DamageType {
		occupied,
		pwn,
		down
	}

	public Color GetRealWorldColorByName(string color) {
		Color toColor = Color.white;
		switch (color) {
		case "red":
			toColor = Color.red;
			break;
		case "yellow":
			toColor = Color.yellow;
			break;
		case "blue":
			toColor = new Color(0.3f, 0.3f, 1f);
			break;
		case "orange":
            toColor = new Color(0.64f, 0.33f, 0.218f);
			break;
		case "purple":
			toColor = new Color (0.5f, 0f, 0.5f);
			break;
		case "green":
			toColor = Color.green;
			break;
		case "cyan":
			toColor = Color.cyan;
			break;
		case "magenta":
			toColor = Color.magenta;
			break;
		case "red2":
			toColor = new Color (0xCC / 255f, 0x33 / 255f, 0x33 / 255f, 0xCC / 255f);
			break;
		case "yellow2":
			toColor = new Color (0xCC / 255f, 0xC8 / 255f, 0x31 / 255f, 0xCC / 255f);
			break;
		case "blue2":
			toColor = new Color (0x33 / 255f, 0x60 / 255f, 0xCC / 255f, 0xCC / 255f);
			break;
		case "orange2":
			toColor = new Color (0xCC / 255f, 0x61 / 255f, 0x31 / 255f, 0xCC / 255f);
			break;
		case "purple2":
			toColor = new Color (0x36 / 255f, 0x31 / 255f, 0xCC / 255f, 0xCC / 255f);
			break;
		case "green2":
			toColor = new Color (0x33 / 255f, 0xCC / 255f, 0x4D / 255f, 0xCC / 255f);
			break;
		}
		return toColor;
	}

//	public Color GetRealWorldColorByName(string color) {
//		Color c = Color.white;
//		switch (color) {
//		case "red":
//			c = new Color (0xCC / 255f, 0x33 / 255f, 0x33 / 255f, 0xCC / 255f);
//			break;
//		case "yellow":
//			c = new Color (0xCC / 255f, 0xC8 / 255f, 0x31 / 255f, 0xCC / 255f);
//			break;
//		case "blue":
//			c = new Color (0x33 / 255f, 0x60 / 255f, 0xCC / 255f, 0xCC / 255f);
//			break;
//		case "orange":
//			c = new Color (0xCC / 255f, 0x61 / 255f, 0x31 / 255f, 0xCC / 255f);
//			break;
//		case "purple":
//			c = new Color (0x36 / 255f, 0x31 / 255f, 0xCC / 255f, 0xCC / 255f);
//			break;
//		case "green":
//			c = new Color (0x33 / 255f, 0xCC / 255f, 0x4D / 255f, 0xCC / 255f);
//			break;
//		}
//		return c;
//	}

}
