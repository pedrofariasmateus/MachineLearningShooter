using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Accord.IO;

namespace MachineLearningShooter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Jogo : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //objectos e texturas do jogo
        Nave nave1, nave2;
        String temporizador1, temporizador2;
        Texture2D texturaBarraVida, texturaBarraEscudo;
        Rectangle drawBarraVida1, drawBarraVida2, drawBarraEscudo1, drawBarraEscudo2, drawGameOver, drawButtonSim, drawButtonNao;
        static List<Bala> balas = new List<Bala>();
        SpriteFont font;
        Vector2 posVida1, posVida2, posEscudoTempo1, posEscudoTempo2, posPontos1, posPontos2;
        CsvWriter writeDataSet;
        CsvReader loadDataSet;
        TextWriter bufferWriter;
        TextReader bufferReader;

        //variaveis do jogo
        int vidaNave1, vidaNave2, escudoNave1, escudoNave2, pontosNave1, pontosNave2, numeroRondas;
        bool escudoActivoNave1, escudoActivoNave2;

        //sons do jogo
        //ainda por fazer no fim

        public Jogo()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //tamanho da janela
            graphics.PreferredBackBufferHeight = ConstantesDoJogo.WINDOW_HEIGHT;
            graphics.PreferredBackBufferWidth = ConstantesDoJogo.WINDOW_WIDTH;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //carregar as texturas
            texturaBarraVida = Content.Load<Texture2D>("barra_hp");
            texturaBarraEscudo = Content.Load<Texture2D>("barra_escudo");
            
            //criar as naves
            nave1 = new Nave(this.Content,null,ConstantesDoJogo.WINDOW_WIDTH/2,20,Jogador.Jogador1);
            escudoActivoNave1 = nave1.EscudoActivo;

            nave2 = new Nave(this.Content, null, ConstantesDoJogo.WINDOW_WIDTH / 2, ConstantesDoJogo.WINDOW_HEIGHT - 120,Jogador.Jogador2);
            escudoActivoNave2 = nave2.EscudoActivo;

            //criar as barras
            drawBarraVida1 = new Rectangle(0, 0, texturaBarraVida.Width, 20);
            drawBarraVida2 = new Rectangle(0, ConstantesDoJogo.WINDOW_HEIGHT - 20, ConstantesDoJogo.WINDOW_WIDTH, 20);
            drawBarraEscudo1 = new Rectangle(0, 0, ConstantesDoJogo.WINDOW_WIDTH, 20);
            drawBarraEscudo2 = new Rectangle(0, ConstantesDoJogo.WINDOW_HEIGHT - 20, ConstantesDoJogo.WINDOW_WIDTH, 20);

            //carregar a letra dos temporizadores
            font = Content.Load<SpriteFont>("Arial");
            posVida1 = new Vector2(ConstantesDoJogo.WINDOW_WIDTH/2, 0);
            posVida2 = new Vector2(ConstantesDoJogo.WINDOW_WIDTH/2, ConstantesDoJogo.WINDOW_HEIGHT-20);
            posEscudoTempo1 = new Vector2(ConstantesDoJogo.WINDOW_WIDTH - 20, 20);
            posEscudoTempo2 = new Vector2(20, ConstantesDoJogo.WINDOW_HEIGHT - 40);
            posPontos1 = new Vector2(20, 20);
            posPontos2 = new Vector2(ConstantesDoJogo.WINDOW_WIDTH - 20, ConstantesDoJogo.WINDOW_HEIGHT-40);
            numeroRondas = 5;

            //inicializar leitor de CSV e abrir CSV (se existir)

            //inicializar escritor de CSV 

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboard= Keyboard.GetState();

            // Allows the game to exit
            if (keyboard.IsKeyDown(Keys.Escape)) {

                //guardar ou adicionar dados escritos no CSV

                //fechar jogo
                this.Exit();
            }
                

            // TODO: Add your update logic here
            
            //actualizar as naves
            nave1.Update(gameTime, keyboard);
            nave2.Update(gameTime, keyboard);

            //actualizar os temporizadores
            temporizador1 = nave1.TempoEscudo.ToString();
            temporizador2 = nave2.TempoEscudo.ToString();

            //actualizar as balas
            foreach (Bala bala in balas) 
            {
                bala.Update(gameTime);
                if (bala.Y > ConstantesDoJogo.WINDOW_HEIGHT || bala.Y < 0)
                    bala.Visivel = false;
            }

            foreach (Bala bala in balas)
            {
                if (!nave1.EscudoActivo)
                {
                    if (nave1.Forma.Intersects(bala.Forma) && nave1.Visivel && bala.Pertence == Jogador.Jogador2)
                    {
                        if (IntersectPixels(nave1.Forma, nave1.TexturaNave, bala.Forma, bala.TexturaBala))
                        {
                            nave1.Vida -= bala.Dano;
                            bala.Visivel = false;
                            drawBarraVida1.Width = nave1.PercentagemVidaBarra;
                        }

                    }
                }
                else
                {
                    if (nave1.EscudoForma.Intersects(bala.Forma) && nave1.EscudoActivo && bala.Pertence == Jogador.Jogador2)
                    {
                        if (IntersectPixels(nave1.EscudoForma, nave1.TexturaEscudo, bala.Forma, bala.TexturaBala))
                        {
                            nave1.Escudo -= bala.Dano;
                            bala.Visivel = false;
                            drawBarraEscudo1.Width = nave1.PercentagemEscudoBarra;
                        }

                    }
                }

                if (!nave2.EscudoActivo)
                {
                    if (nave2.Forma.Intersects(bala.Forma) && nave2.Visivel && bala.Pertence == Jogador.Jogador1)
                    {
                        if (IntersectPixels(nave2.Forma, nave2.TexturaNave, bala.Forma, bala.TexturaBala))
                        {
                            nave2.Vida -= bala.Dano;
                            bala.Visivel = false;
                            drawBarraVida2.Width = nave2.PercentagemVidaBarra;
                        }

                    }
                }
                else
                {
                    if (nave2.EscudoForma.Intersects(bala.Forma) && nave2.EscudoActivo && bala.Pertence == Jogador.Jogador1)
                    {
                        if (IntersectPixels(nave2.EscudoForma, nave2.TexturaEscudo, bala.Forma, bala.TexturaBala))
                        {
                            nave2.Escudo -= bala.Dano;
                            bala.Visivel = false;
                            drawBarraEscudo2.Width = nave2.PercentagemEscudoBarra;
                        }

                    }
                }
            }

            //limpar as balas "invisiveis"
            for (int i = 0; i < balas.Count; i++)
                if (balas.ElementAt(i).Visivel == false)
                    balas.RemoveAt(i);

            //fazer as naves "invisiveis" quando morrem
            if (nave1.Vida <= 0)
            {
                nave1.Visivel = false;
            }
            if (nave2.Vida <= 0)
            {
                nave2.Visivel = false;
            }

            if (nave1.EscudoActivo && nave1.Escudo <= 0)
            {
                nave1.EscudoActivo = false;
                drawBarraEscudo1.Width = ConstantesDoJogo.WINDOW_WIDTH;
            }

            if (nave2.EscudoActivo && nave2.Escudo <= 0)
            {
                nave2.EscudoActivo = false;
                drawBarraEscudo2.Width = ConstantesDoJogo.WINDOW_WIDTH;
            }
            
            //escrever linha no CSV

            if (nave1.Visivel == false || nave2.Visivel == false)
            {
                nave1.Desactivado = true;
                nave2.Desactivado = true;
                
                if (nave1.Visivel == false)
                    pontosNave2++;
                
                else
                    pontosNave1++;
                
                limparBalas();

                //if (pontosNave1 == numeroRondas)
                //{
                //    return;
                //}

                //if (pontosNave2 == numeroRondas)
                //{
                //    return;
                //}


                nave1.Reset();
                nave2.Reset();
                drawBarraVida1.Width = ConstantesDoJogo.WINDOW_WIDTH;
                drawBarraVida2.Width = ConstantesDoJogo.WINDOW_WIDTH;

            }


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Aqua);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            
            //desenhar as naves se estiverem visiveis/vivas
            if(nave1.Visivel==true)
                nave1.Draw(spriteBatch);
            if(nave2.Visivel==true)
                nave2.Draw(spriteBatch);

            //desenhar as balas
            foreach (Bala bala in balas)
                bala.Draw(spriteBatch);

            
            //desenhar as barras de hp e de escudo(se activo)
            spriteBatch.Draw(texturaBarraVida, drawBarraVida1, Color.White);
            spriteBatch.Draw(texturaBarraVida, drawBarraVida2, Color.White);

            //desenhar os valores de vida da nave
            
            if (nave1.EscudoActivo) { 
                spriteBatch.Draw(texturaBarraEscudo, drawBarraEscudo1, Color.White);
                spriteBatch.DrawString(font, nave1.PercentagemEscudo.ToString(), posVida1, Color.Blue);
            }
            else
                spriteBatch.DrawString(font, nave1.PercentagemVida.ToString(), posVida1, Color.Green);

            if (nave2.EscudoActivo) { 
                spriteBatch.Draw(texturaBarraEscudo, drawBarraEscudo2, Color.White);
                spriteBatch.DrawString(font, nave2.PercentagemEscudo.ToString(), posVida2, Color.Blue);
            }
            else
                spriteBatch.DrawString(font, nave2.PercentagemVida.ToString(), posVida2, Color.Green);

            //desenhar o tempo de arrefecimento do escudo
            spriteBatch.DrawString(font, temporizador1, posEscudoTempo1, Color.White);
            spriteBatch.DrawString(font, temporizador2, posEscudoTempo2, Color.White);

            //desenhar os pontos de cada nave
            spriteBatch.DrawString(font, pontosNave1.ToString(), posPontos1, Color.Black);
            spriteBatch.DrawString(font, pontosNave2.ToString(), posPontos2, Color.Black);

            

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public static void AdicionarBala(Bala bala)
        {
            balas.Add(bala);
        }

        public static void limparBalas()
        {
            balas.Clear();
        }


        //algoritmo detector de colisao de imagens
        static bool IntersectPixels(Rectangle rectangleA, Texture2D texA,
                            Rectangle rectangleB, Texture2D texB)
        {
            // Find the bounds of the rectangle intersection
            int x1 = Math.Max(rectangleA.X, rectangleB.X);
            int x2 = Math.Min(rectangleA.X + rectangleA.Width, rectangleB.X + rectangleB.Width);
            int y1 = Math.Max(rectangleA.Y, rectangleB.Y);
            int y2 = Math.Min(rectangleA.Y + rectangleA.Height, rectangleB.Y + rectangleB.Height);

            // Get Color data of each Texture
            Color[] dataA = new Color[texA.Width * texA.Height];
            texA.GetData(dataA);
            Color[] dataB = new Color[texB.Width * texB.Height];
            texB.GetData(dataB);

            // Check every point within the intersection bounds
            for (int y = y1; y < y2; y++)
            {
                for (int x = x1; x < x2; x++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = dataA[(x - rectangleA.X) +
                                         (y - rectangleA.Y) * texA.Width];
                    Color colorB = dataB[(x - rectangleB.X) +
                                         (y - rectangleB.Y) * texB.Width];

                    // If both pixels are not completely transparent,
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        // then an intersection has been found
                        return true;
                    }
                }
            }

            // No intersection found
            return false;
        }
    }
}
