using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace CreativeSpore
{
	[ExecuteInEditMode]
	public class CharAnimationController : MonoBehaviour 
	{
		public enum eDir
		{
			DOWN,
			LEFT,
			RIGHT,
			UP
		}

        Vector3[] m_dirVect = new Vector3[]
        {
            new Vector3(0, -1), //DOWN
            new Vector3(-1, 0), //LEFT
            new Vector3(1, 0), //RIGHT
            new Vector3(0, 1), //UP
        };

		public Sprite SpriteCharSet;
		public SpriteRenderer TargetSpriteRenderer;
		public Vector2 Pivot = new Vector2( 0.5f, 0f );
		public eDir CurrentDir = eDir.DOWN;
        public Vector3 CurrentDirVect { get { return m_dirVect[(int)CurrentDir]; } }
        public bool IsPingPongAnim = true; // set true for ping pong animation
		public bool IsAnimated = true;
		public float AnimSpeed = 9f; // frames per second
        public int AnimFrames = 3; // how many frames have any animation ( default 3 for VX Character, use 4 for XP Characters )

        [SerializeField]
        public int CurrentFrame
        {
            get
            {
                int curFrame = m_internalFrame;
                if (IsPingPongAnim && m_isPingPongReverse)
                {
                    curFrame = AnimFrames - 1 - curFrame;
                }
                return curFrame;
            }
        }

		[SerializeField]
		private List<Sprite> SpriteFrames = new List<Sprite>();

        [SerializeField]
        private int m_internalFrame; // frame counter used internally. CurFrame would be the real animation frame
        private float m_curFrameTime;
        private bool m_isPingPongReverse = false;

		void Awake()
		{
			CreateSpriteFrames();
		}

		// Use this for initialization
		void Start () 
		{
            if (TargetSpriteRenderer == null)
            {
                TargetSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }
		}

        void Update()
        {
            if (IsAnimated)
            {
                m_curFrameTime += Time.deltaTime;
                while (m_curFrameTime > (1 / AnimSpeed))
                {
                    int frameIdx = CurrentFrame;
                    float frameTime = 1 / AnimSpeed;
                    if (IsPingPongAnim && (frameIdx == 0 || frameIdx == (AnimFrames-1)))
                    {
                        frameTime /= 2; // avoid stay twice of the time in the first and last frame of the animation
                    }
                    m_curFrameTime -= frameTime;
                    ++m_internalFrame; m_internalFrame %= AnimFrames;
                    if (m_internalFrame == 0)
                    {
                        m_isPingPongReverse = !m_isPingPongReverse;
                    }
                }
            }
            else
            {
                m_internalFrame = 0;
            }

            TargetSpriteRenderer.sprite = SpriteFrames[(int)((int)CurrentDir * AnimFrames + CurrentFrame)];
        }

		[ContextMenu ("CreateSpriteFrames")]
		public void CreateSpriteFrames()
		{
			if( SpriteCharSet != null )
			{
				SpriteFrames.Clear();
                int frameWidth = (int)SpriteCharSet.rect.width / AnimFrames;
                int frameHeight = (int)SpriteCharSet.rect.height / 4;
				int frameNb = 0;
				Rect rFrame = new Rect(0f, 0f, (float)frameWidth, (float)frameHeight);
                for (rFrame.y = SpriteCharSet.rect.yMax - frameHeight; rFrame.y >= SpriteCharSet.rect.y; rFrame.y -= frameHeight)
				{
                    for (rFrame.x = SpriteCharSet.rect.x; rFrame.x < SpriteCharSet.rect.xMax; rFrame.x += frameWidth, ++frameNb)
					{
						Sprite sprFrame = Sprite.Create( SpriteCharSet.texture, rFrame, Pivot, 100f);
						sprFrame.name = SpriteCharSet.name + "_" + frameNb;
						SpriteFrames.Add( sprFrame );
					}
				}

				if( TargetSpriteRenderer != null )
				{
					TargetSpriteRenderer.sprite = SpriteFrames[0];
				}
			}
		}
	}
}