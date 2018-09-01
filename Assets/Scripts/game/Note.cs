﻿using System;
using System.Diagnostics;
using UnityEngine;
using Lf = Game.LiveNotesFunctions;
using Constants;
using Debug = UnityEngine.Debug;

namespace Game
{
    public class Note : MonoBehaviour
    {
        public long TargetTime { private get; set; }
        public Stopwatch Timer { private get; set; }
        public int Lane { private get; set; }
        public Action GetButtonDownFunction { get; set; }

        // Update is called once per frame
        private void Update ()
        {
            //描画される位置を計算
            var position = Lf.CalcuSpawnPosition(Timer.ElapsedTicks, TargetTime);
            //配置
            gameObject.GetComponent<RectTransform>().localPosition = new Vector2(gameObject.GetComponent<RectTransform>().localPosition.x, GameConstants.JUDGE_LINE_Y + position);

            //ボタン
            JudgeByButtonInput();
            
            //目標タイムを越したら...
            if (TargetTime < Timer.ElapsedTicks)
            {
                IsPassedTargetTime();
            }
			
        }

        private void IsPassedTargetTime()
        {
            //線上で待機
            if (TargetTime + GameConstants.NOTE_IS_ON_LINE_WAIT_TIME < Timer.ElapsedTicks)
            {
                GameParameters.LaneQueue[Lane].RemoveAt(0);
                Destroy(gameObject);
            }
            else
            {
                gameObject.GetComponent<RectTransform>().localPosition = new Vector2(gameObject.GetComponent<RectTransform>().localPosition.x, GameConstants.JUDGE_LINE_Y);
            }
        }

        private void JudgeByButtonInput()
        {
            if (GameParameters.LaneQueue[Lane][0] == gameObject && Input.GetButtonDown(GameConstants.KEY_NAME[Lane]))
            {

                if (GameConstants.JUDGE_PERFECT + Timer.ElapsedTicks > TargetTime && GameConstants.JUDGE_PERFECT - TargetTime < Timer.ElapsedTicks)
                {
                    GameParameters.LaneQueue[Lane].RemoveAt(0);
                    Lf.SetJudgeText("Perfect!!!");
                    Destroy(gameObject);
                }
                else if (GameConstants.JUDGE_GREAT + Timer.ElapsedTicks > TargetTime && GameConstants.JUDGE_GREAT - TargetTime < Timer.ElapsedTicks)
                {
                    GameParameters.LaneQueue[Lane].RemoveAt(0);
                    Lf.SetJudgeText("Great!!");
                    Destroy(gameObject);
                } 
                else if (GameConstants.JUDGE_GOOD + Timer.ElapsedTicks > TargetTime && GameConstants.JUDGE_GOOD - TargetTime < Timer.ElapsedTicks)
                {
                    GameParameters.LaneQueue[Lane].RemoveAt(0);
                    Lf.SetJudgeText("Good!");
                    Destroy(gameObject);
                } 
                else if (GameConstants.JUDGE_MISS + Timer.ElapsedTicks > TargetTime && GameConstants.JUDGE_MISS - TargetTime < Timer.ElapsedTicks)
                {
                    GameParameters.LaneQueue[Lane].RemoveAt(0);
                    Lf.SetJudgeText("Miss...");
                    Destroy(gameObject);
                }
                
            }
        }
        
    }
    
}