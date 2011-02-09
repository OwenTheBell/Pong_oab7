using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pong.PongClasses;

namespace Pong.PaddleControllers
{
    /// <summary>
    /// Used to create IPaddleController objects.
    /// </summary>
    public static class PaddleControllerFactory
    {
        /// <summary>
        /// Creates a new IPaddleController based on the specified
        /// pongWorld, aiType, and paddle.
        /// </summary>
        /// <param name="pongWorld">The pong world.</param>
        /// <param name="aiType">Type of the ai.</param>
        /// <param name="paddle">The paddle.</param>
        /// <returns>
        /// The IPaddleController corresponding to the paddle.
        /// </returns>
        public static IPaddleController Create(PongWorld pongWorld, AIType aiType, Paddle paddle)
        {
            switch (aiType)
            {
                case AIType.Human:
                    return new HumanPaddleController(pongWorld, paddle);
                case AIType.PurePursuit:
                    return new PurePursuitPaddleController(pongWorld, paddle);
                case AIType.LeadPursuit:
                    return new LeadPursuitPaddleController(pongWorld, paddle);
                case AIType.Random:
                    return new RandomPaddleController(pongWorld, paddle);
            }

            return null;
        }

        /// <summary>
        /// Creates a new IPaddleController based on the specified
        /// pongWorld and paddle. The AI will be a random non-learner.
        /// </summary>
        /// <param name="pongWorld">The pong world.</param>
        /// <param name="paddle">The paddle.</param>
        /// <returns>
        /// The IPaddleController corresponding to the paddle.
        /// </returns>
        public static IPaddleController Create(PongWorld pongWorld, Paddle paddle)
        {
            int random = new Random().Next(3);
            switch (random)
            {
                case 0:
                    return Create(pongWorld, AIType.PurePursuit, paddle);
                case 1:
                    return Create(pongWorld, AIType.Random, paddle);
                case 2:
                    return Create(pongWorld, AIType.LeadPursuit, paddle);
            }

            return null;
        }
    }
}
