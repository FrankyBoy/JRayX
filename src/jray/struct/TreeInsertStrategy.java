package jray.struct;

public enum TreeInsertStrategy{
	LEAF_ONLY, //Objects are kept at leaves only (memory intensive because of duplication) (and little computational overhead ~x2)
	FIT_INTO_BOX, //Optimal memory footprint but computationally intensive
	DYNAMIC, //Balanced (big objects are not duplicated)
	DYNAMIC_TEST, //experimental (dynamic with count-dependent ratio)
	FAST_BUILD_TEST; //experimental (LEAF_ONLY with with count-dependent ratio)
	
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
	public static final double DYNAMIC_DUPLICATE_MAX_SIZE_RATIO = 0.2;
	
	/**
	 * The minimum box size for TreeInsertStrategy.DYNAMIC
	 */
	public static final double DYNAMIC_MIN_WIDTH = 1e-3;
	
	/**
	 * Default minimum box size for most strategies.
	 */
	public static final int MIN_WIDTH = 4;
	
	/**
	 * Maximal object size per node. If a node contains more objects than MAX_ELEMENTS are
	 * split if allowed by MIN_WIDTH.
	 */
	public static final int MAX_ELEMENTS = 10;
	
	/**
	 * Fault-tolerance for box-intersections  
	 */
	public static final double EPS = 1e-9;
};
