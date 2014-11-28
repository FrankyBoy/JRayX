namespace JRayXLib.Struct
{
    public enum TreeInsertStrategy
    {
        LeafOnly,       //Objects are kept at leaves only (memory intensive because of duplication) (and little computational overhead ~x2)
        FitIntoBox,     //Optimal memory footprint but computationally intensive
        Dynamic,        //Balanced (big objects are not duplicated)
        DynamicTest,    //experimental (dynamic with count-dependent ratio)
        FastBuildTest,  //experimental (LEAF_ONLY with with count-dependent ratio)
    }

    public static class TreeInsertStrategyConstants
    {
        /**
         * The maximum duplication ratio for TreeInsertStrategy.DYNAMIC
         * 
         * If an object's size is a lot smaller than the current box, but is not enclosed by a
         * child-box (because it is intersecting a box-border) it will be duplicated to all children
         * intersecting the given object. This Ration states when objects are small enough to be duplicated.
         * 
         * This optimization is necessary because big objects would be duplicated very often (they would
         * intersect a lot of leaf-boxes).
         */
        public static double DynamicDuplicateMaxSizeRatio = 0.2;

        /**
         * The minimum box size for TreeInsertStrategy.DYNAMIC
         */
        public static double DynamicMinWidth = 1e-3;

        /**
         * Default minimum box size for most strategies.
         */
        public static int MinWidth = 4;

        /**
         * Maximal object size per node. If a node contains more objects than MAX_ELEMENTS are
         * split if allowed by MIN_WIDTH.
         */
        public static int MaxElements = 10;
    };
}