#include <iostream>
#include <vector>
#include <string>
#include <algorithm>

class Item {
public:
    std::string name;
    int weight;
    int value;

    Item(std::string name, int weight, int value) 
        : name(std::move(name)), weight(weight), value(value) {}
};

std::pair<int, std::vector<Item>> knapsack(const std::vector<Item>& items, int maxCapacity) {
    int n = items.size();
    std::vector<std::vector<int>> dp(n + 1, std::vector<int>(maxCapacity + 1, 0));
    std::vector<std::vector<bool>> keep(n + 1, std::vector<bool>(maxCapacity + 1, false));

    // Fill the DP table
    for (int i = 1; i <= n; ++i) {
        for (int w = 0; w <= maxCapacity; ++w) {
            if (items[i - 1].weight <= w) {
                if (dp[i - 1][w] < dp[i - 1][w - items[i - 1].weight] + items[i - 1].value) {
                    dp[i][w] = dp[i - 1][w - items[i - 1].weight] + items[i - 1].value;
                    keep[i][w] = true;
                } else {
                    dp[i][w] = dp[i - 1][w];
                }
            } else {
                dp[i][w] = dp[i - 1][w];
            }
        }
    }

    // Find the selected items
    std::vector<Item> selectedItems;
    int totalWeight = maxCapacity;
    for (int i = n; i > 0; --i) {
        if (keep[i][totalWeight]) {
            selectedItems.push_back(items[i - 1]);
            totalWeight -= items[i - 1].weight;
        }
    }

    std::reverse(selectedItems.begin(), selectedItems.end());
    return { dp[n][maxCapacity], selectedItems };
}

int main() {
    const int MAX_CAPACITY = 6404180;
    std::vector<Item> items = {
        {"Axe", 32252, 68674},
        {"Bronze coin", 225790, 471010},
        {"Crown", 468164, 944620},
        {"Diamond statue", 489494, 962094},
        {"Emerald belt", 35384, 78344},
        {"Fossil", 265590, 579152},
        {"Gold coin", 497911, 902698},
        {"Helmet", 800493, 1686515},
        {"Ink", 823576, 1688691},
        {"Jewel box", 552202, 1056157},
        {"Knife", 323618, 677562},
        {"Long sword", 382846, 833132},
        {"Mask", 44676, 99192},
        {"Necklace", 169738, 376418},
        {"Opal badge", 610876, 1253986},
        {"Pearls", 854190, 1853562},
        {"Quiver", 671123, 1320297},
        {"Ruby ring", 698180, 1301637},
        // {"Silver bracelet", 446517, 859835},
        // {"Timepiece", 909620, 1677534},
        // {"Uniform", 904818, 1910501},
        // {"Venom potion", 730061, 1528646},
        // {"Wool scarf", 931932, 1827477},
        // {"Crossbow", 952360, 2068204},
        // {"Yesteryear book", 926023, 1746556},
        // {"Zinc cup", 978724, 2100851}
    };

    auto [maxValue, selectedItems] = knapsack(items, MAX_CAPACITY);

    std::cout << "Maximum value: " << maxValue << std::endl;
    std::cout << "Selected items:" << std::endl;
    for (const auto& item : selectedItems) {
        std::cout << item.name << ": Weight = " << item.weight << ", Value = " << item.value << std::endl;
    }

    return 0;
}
