using System;

namespace arkanoid_component
{
    public static class OptionsManager
    {
        // Domyślne wartości
        public static int Lives { get; set; } = 3;
        public static int PointsPerBrick { get; set; } = 500;

        // (opcjonalnie) tu możesz dodać Load/Save do pliku, jeśli chcesz persistować
    }
}
