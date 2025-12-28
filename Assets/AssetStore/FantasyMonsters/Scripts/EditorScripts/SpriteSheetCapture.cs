using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.FantasyMonsters.Scripts.Utils;
using UnityEngine;

namespace Assets.FantasyMonsters.Scripts.EditorScripts
{
    /// <summary>
    /// Used for creating sprite sheets for frame-by-frame animation.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class SpriteSheetCapture : MonoBehaviour
    {
        public Monster Monster;

        private readonly Dictionary<string, List<Texture2D>> _clips = new Dictionary<string, List<Texture2D>>();

        public void Capture(int frameSize, int frameCount)
        {
            Monster = FindObjectOfType<Monster>();
            StartCoroutine(CaptureCoroutine(frameSize, frameCount));
        }

        private IEnumerator CaptureCoroutine(int frameSize, int frameCount)
        {
            foreach (var stateHandler in Monster.Animator.GetBehaviours<StateHandler>())
            {
                stateHandler.StateExit.RemoveAllListeners();
            }

            var order = new List<string> { "Ready", "Walk", "Attack", "Death" };
            var clips = Monster.Animator.runtimeAnimatorController.animationClips.Select(i => i.name).Distinct().OrderBy(i => order.IndexOf(i)).ToList();
            
            yield return CaptureFrames(clips, frameSize, frameCount, 0.1f);
        }

        private IEnumerator CaptureFrames(List<string> clips, int frameSize, int frameCount, float interval)
        {
            _clips.Clear();

            foreach (var clip in clips)
            {
                Monster.SetHead(0);

                _clips.Add(clip, new List<Texture2D>());

                for (var i = 0; i < frameCount; i++)
                {
                    if (clip == "Death")
                    {
                        yield return ShowFrame(clip, (float) i / (frameCount - 1));
                    }
                    else
                    {
                        yield return ShowFrame(clip, (float) i / frameCount);
                    }

                    yield return new WaitForSeconds(interval);

                    var texture = CaptureFrame(frameSize, frameSize);

                    _clips[clip].Add(texture);
                }
            }

            CreateSheet();
        }

        private IEnumerator ShowFrame(string clipName, float normalizedTime)
        {
            Monster.Animator.Play(clipName, 0, normalizedTime);
            Monster.Animator.speed = 0;

            yield return null;

            while (Monster.Animator.GetCurrentAnimatorClipInfo(0).Length == 0)
            {
                yield return null;
            }

            var clipInfo = Monster.Animator.GetCurrentAnimatorClipInfo(0);

            if (clipInfo.Length == 0) throw new Exception("clipInfo.Length == 0");

            var clip = clipInfo[0].clip;

            if (clip.name != clipName) throw new Exception("clip.name != clipName");

            var @event = clip.events.OrderBy(i => Mathf.Abs(i.time - normalizedTime * clip.length)).FirstOrDefault();

            //Debug.Log($"clip={clip.name}, clip.events.Length={clip.events.Length}, @event?.functionName={@event?.functionName}, @event?.intParameter={@event?.intParameter}");

            if (@event != null) switch (@event.functionName)
            {
                case "SetHead":
                    Monster.SetHead(@event.intParameter);
                    break;
            }
        }

        private Texture2D CaptureFrame(int width, int height)
        {
            var cam = GetComponent<Camera>();
            var renderTexture = new RenderTexture(width, height, 24);
            var texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);

            cam.targetTexture = renderTexture;
            cam.Render();
            RenderTexture.active = renderTexture;
            texture2D.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            cam.targetTexture = null;
            RenderTexture.active = null;
            Destroy(renderTexture);

            return texture2D;
        }

        private void CreateSheet()
        {
            var textures = _clips.SelectMany(i => i.Value).ToList();
            var rects = textures.Select(TextureHelper.GetContentRect).ToList();
            var xMin = (int) rects.Min(i => i.xMin);
            var yMin = (int) rects.Min(i => i.yMin);
            var xMax = (int) rects.Max(i => i.xMax);
            var yMax = (int) rects.Max(i => i.yMax);
            var width = xMax - xMin;
            var height = yMax - yMin;
            var sheet = new Texture2D(_clips.First().Value.Count * width, _clips.Keys.Count * height);
            
            foreach (var clip in _clips)
            {
                textures = clip.Value;

                for (var i = 0; i < textures.Count; i++)
                {
                    sheet.SetPixels(i * width, _clips.Keys.Reverse().ToList().IndexOf(clip.Key) * height, width, height, textures[i].GetPixels(xMin, yMin, xMax - xMin, yMax - yMin));
                }
            }

            sheet.Apply();
            
            var bytes = sheet.EncodeToPNG();
            var fileName = $"{Monster.name} {width}x{height} ({string.Join(" ", _clips.Keys).Replace("Ready", "Idle").Replace("Walk", "Run")})";
            
            #if UNITY_EDITOR

            var path = UnityEditor.EditorUtility.SaveFilePanel("Save as", Application.dataPath, $"{fileName}", "png");

            if (path != "")
            {
                File.WriteAllBytes(path, bytes);
                Debug.Log($"Saved as {path}");
            }

            #endif

            textures.ForEach(Destroy);
            Destroy(sheet);
        }
    }
}