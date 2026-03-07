//Giving you an array and target, return indices of two numbers that sum to target
using System


//testdata[2,3,11,15],9)


void Main() {
    int[] testdata = {2,3,11,15,9};
    int[] twosum(int[] nums, int target )
    {
    
        var map = new Dictionary<int, int>();
        for (int i = 0; i< nums.Length; i++)
        {
            int complement = target - nums[i];
            if (map.ContainsKey(complement))
            {
                return new int[] {map[complement],i};
            }
            if (!map.ContainsKey(nums[i]))
            {
                map[nums[i]]=i;
            }
        }
        return Array.Empty<int>();

    }
    var result = twosum(testdata, 11);
    Console.WriteLine($"[{result[0]}], {result[1]}]");

}