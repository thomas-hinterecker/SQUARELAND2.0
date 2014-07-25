using UnityEngine;
using System;
using System.Xml;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace SquarelandSystem {

	public class Maze {

		public static float gapBetweenBlocks;

		public float eyeHeight;

		public CameraSetting cameraSetting;
		public GeometrieSetting geometrieSetting;
		public TexturesSetting texturesSetting;

		protected GameObject blockToCloneObject;
		protected GameObject waypointToCloneObject;
		protected GameObject floorObject;
		protected Material cubeMaterial;
		protected Material landmarkMaterial;

		protected GameObject border1;
		protected GameObject border2;
		protected GameObject border3;
		protected GameObject border4;

		public static float blockWidth;
		public float blockHeight;
		public float blockLandmarkWidth;
		public float blockLandmarkHeight;

		public Maze (Material cube_material, Material landmark_material, GameObject waypoint_to_clone_object, GameObject floor_object) {
			geometrieSetting = (GeometrieSetting) Controller.settings["geometrie"];
			texturesSetting = (TexturesSetting) Controller.settings["textures"];
			cameraSetting = (CameraSetting) Controller.settings["camera"];

			gapBetweenBlocks = geometrieSetting.gapBetweenBlocks;
			blockWidth = geometrieSetting.blockWidth;
			blockHeight = geometrieSetting.blockHeight;
			blockLandmarkWidth = geometrieSetting.blockLandmarkWidth;
			blockLandmarkHeight = geometrieSetting.blockLandmarkHeight;


			cameraSetting.ApplyCameraSetting();
			eyeHeight = cameraSetting.eyeHeight;

			/*switch (geometrieSetting.blockType) {
			case "fullLandmarkSize":
				blockToCloneObject = block_full_landmark_size;
				break;
			case "smallerLandmarkSize":
				blockToCloneObject = block_smaller_landmark_size;
				break;
			default:
				blockToCloneObject = block_full_landmark_size;
				break;
			}*/

			waypointToCloneObject = waypoint_to_clone_object;
			floorObject = floor_object;
			cubeMaterial = cube_material;
			landmarkMaterial = landmark_material;
		}

		public void CreateMaze () {
			CreateGrid();
		}

		protected void CreateGrid () {
			GameObject cloned_block, cloned_waypoint;
			GameObject blocks_parent_object = new GameObject("BlocksParent");
			GameObject waypoints_parent_object = new GameObject("WaypointsParent");
			int row, column;
			float position_z, position_x;
			position_z = position_x = 0;

			CreatePrototypicalCube(blocks_parent_object);
			if (true == geometrieSetting.borders) {
				CreateBorder();
			}
			ModifyTextures();
			
			for (row = geometrieSetting.mazeRow; row > 0; --row) {
				position_x = 0;
				for (column = 1; column <= geometrieSetting.mazeColumn; ++column) {
					if (row == geometrieSetting.mazeRow && column == 1) {

					} else {
						cloned_block = UnityEngine.Object.Instantiate(blockToCloneObject) as GameObject;
						cloned_block.name = "Block_" + row.ToString() + "_" + column.ToString();
						cloned_block.transform.parent = blocks_parent_object.transform;
						cloned_block.transform.position = new Vector3(
							position_x,
							(blockHeight) / 2,
							position_z
						);

						/*if (row == geometrieSetting.mazeRow / 2 && column == geometrieSetting.mazeColumn / 2) {
							floor_x = position_x;
							floor_z = position_z;
						}*/

						if (column < geometrieSetting.mazeColumn && row > 1) {
							cloned_waypoint = UnityEngine.Object.Instantiate(waypointToCloneObject) as GameObject;
							cloned_waypoint.name = "Waypoint_" + (row - 1).ToString() + "_" + column.ToString();
							cloned_waypoint.transform.parent = waypoints_parent_object.transform;
							cloned_waypoint.transform.position = new Vector3(
								position_x + (blockWidth / 2) + (gapBetweenBlocks / 2),
								eyeHeight,
								position_z + (blockWidth / 2) + (gapBetweenBlocks / 2)
							);
							cloned_waypoint.transform.renderer.enabled = false;
						}
					}
					position_x += ((blockWidth + gapBetweenBlocks) * 2) / 2;
				}
				position_z += ((blockWidth + gapBetweenBlocks) * 2) / 2;
			}
			//blockToCloneObject.transform.renderer.enabled = false;

			floorObject.transform.position = new Vector3(
				(blockWidth * geometrieSetting.mazeRow + geometrieSetting.gapBetweenBlocks * geometrieSetting.mazeRow) / 2.25f,
				0,
				(blockWidth * geometrieSetting.mazeColumn + geometrieSetting.gapBetweenBlocks * geometrieSetting.mazeColumn) / 2.25f
			);
			floorObject.transform.localScale = new Vector3((blockWidth * geometrieSetting.mazeRow + geometrieSetting.gapBetweenBlocks * geometrieSetting.mazeRow) / 7.5f, 1, (blockWidth * geometrieSetting.mazeColumn + geometrieSetting.gapBetweenBlocks * geometrieSetting.mazeColumn) / 7.5f);

			if (true == geometrieSetting.inDoor) {
				GameObject ceiling = UnityEngine.Object.Instantiate(floorObject) as GameObject;
				ceiling.transform.Translate(Vector3.up * blockHeight); 
				ceiling.transform.Rotate(new Vector3(180, 0, 0));

				byte[] textureData = File.ReadAllBytes("Assets/Textures/System/Ceiling.jpg");
				Texture2D ceiling_texture = new Texture2D(0, 0);
				ceiling_texture.LoadImage(textureData);

				ceiling.renderer.material.mainTexture = ceiling_texture;
				ceiling.name = "ceiling";
			}
		}

		protected void CreatePrototypicalCube (GameObject blocks_parent_object) {
			blockToCloneObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
			blockToCloneObject.name = "Block_" + geometrieSetting.mazeRow + "_1";
			blockToCloneObject.transform.parent = blocks_parent_object.transform;
			blockToCloneObject.renderer.material = cubeMaterial;
			blockToCloneObject.transform.position = new Vector3(0, blockHeight / 2, 0);
			blockToCloneObject.transform.localScale = new Vector3(blockWidth, blockHeight, blockWidth);
			
			AddLandmarkPlane("LM_1", -1.0f, -1.0f, 1);
			AddLandmarkPlane("LM_2", 1.0f, -1.0f, 1);
			
			AddLandmarkPlane("LM_3", -1.0f, 1.0f, 4);
			AddLandmarkPlane("LM_4", 1.0f, 1.0f, 4);

			AddLandmarkPlane("LM_5", 1.0f, 1.0f, 3);
			AddLandmarkPlane("LM_6", -1.0f, 1.0f, 3);

			AddLandmarkPlane("LM_7", 1.0f, -1.0f, 2);
			AddLandmarkPlane("LM_8", -1.0f, -1.0f, 2);
		}
		
		protected void AddLandmarkPlane (string name, float x, float y, int side) {
			float plane_width, plane_height, plane_position_x, plane_position_y;
			plane_position_x = 0;
			plane_position_y = 0;
			
			GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
			plane.transform.parent = blockToCloneObject.transform;
			plane.name = name;
			plane.renderer.enabled = false;
			plane.renderer.material = landmarkMaterial;
			
			plane_width = (blockLandmarkWidth / blockWidth) / 10;
			plane_height = (blockLandmarkHeight / blockHeight) / 10;
			
			switch (side) {
			case 1:
				plane.transform.Rotate(new Vector3(90, 180, 0));
				plane_position_x = (blockWidth / 2 * x) - (blockLandmarkWidth / 2) * x;
				plane_position_y = (blockWidth / 2 * y) + 0.01f * y;
				break;
			case 2:
				plane.transform.Rotate(new Vector3(90, 270, 0));
				plane_position_y = (blockWidth / 2 * x) - (blockLandmarkWidth / 2) * x;
				plane_position_x = (blockWidth / 2 * y) + 0.01f * y;
				break;
			case 3:
				plane.transform.Rotate(Vector3.left * -90);
				plane_position_x = (blockWidth / 2 * x) - (blockLandmarkWidth / 2) * x;
				plane_position_y = (blockWidth / 2 * y) + 0.01f * y;
				break;
			case 4:
				plane.transform.Rotate(new Vector3(90, 90, 0));
				//plane.transform.Rotate(Vector3.up * 90);
				plane_position_y = (blockWidth / 2 * x) - (blockLandmarkWidth / 2) * x;
				plane_position_x = (blockWidth / 2 * y) + 0.01f * y;
				break;
			}
			
			plane.transform.localScale = new Vector3(plane_width, 1, plane_height);
			
			plane.transform.position = new Vector3(plane_position_x, blockHeight / 2, plane_position_y);
		}

		protected void CreateBorder () {
			GameObject borders = new GameObject();
			borders.name = "Borders";

			float length = blockWidth * geometrieSetting.mazeRow + geometrieSetting.gapBetweenBlocks * geometrieSetting.mazeRow + geometrieSetting.gapBetweenBlocks;

			float x = length / 2 - geometrieSetting.gapBetweenBlocks * 3;
			float z = geometrieSetting.gapBetweenBlocks + blockWidth;

			border1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
			border1.name = "Border_1";
			border1.transform.parent = borders.transform;
			border1.renderer.material = cubeMaterial;
			border1.transform.position = new Vector3(x, blockHeight / 2, z * -1);
			border1.transform.localScale = new Vector3(length, blockHeight, blockWidth);

			z = (length - geometrieSetting.gapBetweenBlocks) * -1;

			border2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
			border2.name = "Border_2";
			border2.transform.parent = borders.transform;
			border2.renderer.material = cubeMaterial;
			border2.transform.position = new Vector3(x, blockHeight / 2, z * -1);
			border2.transform.localScale = new Vector3(length, blockHeight, blockWidth);

			z = x * -1;
			x = (geometrieSetting.gapBetweenBlocks + blockWidth) * -1;
			
			border3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
			border3.name = "Border_3";
			border3.transform.parent = borders.transform;
			border3.renderer.material = cubeMaterial;
			border3.transform.position = new Vector3(x, blockHeight / 2, z * -1);
			border3.transform.localScale = new Vector3(length, blockHeight, blockWidth);
			border3.transform.Rotate(new Vector3(0, 90, 0));

			x = (length - geometrieSetting.gapBetweenBlocks);
			
			border4 = GameObject.CreatePrimitive(PrimitiveType.Cube);
			border4.name = "Border_4";
			border4.transform.parent = borders.transform;
			border4.renderer.material = cubeMaterial;
			border4.transform.position = new Vector3(x, blockHeight / 2, z * -1);
			border4.transform.localScale = new Vector3(length, blockHeight, blockWidth);
			border4.transform.Rotate(new Vector3(0, 90, 0));

		}

		protected void ModifyTextures () {
			string[] color_splitted;
			float r = 0;
			float g = 0;
			float b = 0;
			Vector2 texture_tiling;

			if (texturesSetting.type == "color") {
				color_splitted = texturesSetting.blockColor.Split('.');
				r = float.Parse(color_splitted[0]);
				g = float.Parse(color_splitted[1]);
				b = float.Parse(color_splitted[2]);
				Color color = new Color(r/255.0f, g/255.0f, b/255.0f);
				blockToCloneObject.renderer.material.color = color;

				if (true == geometrieSetting.borders) {
					border1.renderer.material.color = color;
					border2.renderer.material.color = color;
					border3.renderer.material.color = color;
					border4.renderer.material.color = color;
				}
			} else {
				texture_tiling = new Vector2(texturesSetting.blockTextureTilingX, texturesSetting.blockTextureTilingY);
				blockToCloneObject.renderer.material.mainTexture = texturesSetting.blockTexture;
				blockToCloneObject.renderer.material.mainTextureScale = texture_tiling;

				if (true == geometrieSetting.borders) {
					border1.renderer.material.mainTexture = texturesSetting.blockTexture;
					border1.renderer.material.mainTextureScale = texture_tiling;

					border2.renderer.material.mainTexture = texturesSetting.blockTexture;
					border2.renderer.material.mainTextureScale = texture_tiling;

					border3.renderer.material.mainTexture = texturesSetting.blockTexture;
					border3.renderer.material.mainTextureScale = texture_tiling;

					border4.renderer.material.mainTexture = texturesSetting.blockTexture;
					border4.renderer.material.mainTextureScale = texture_tiling;
				}
			}
			
			if (texturesSetting.surfaceTexture != null) {
				floorObject.renderer.material.mainTexture = texturesSetting.surfaceTexture;
				floorObject.renderer.material.mainTextureScale = new Vector2(texturesSetting.surfaceTextureTilingX, texturesSetting.surfaceTextureTilingY);
			}
			
			color_splitted = texturesSetting.skyColor.Split('.');
			r = float.Parse(color_splitted[0]);
			g = float.Parse(color_splitted[1]);
			b = float.Parse(color_splitted[2]);
			Camera.main.backgroundColor = new Color(r/255.0f, g/255.0f, b/255.0f);
		}
	}
}
