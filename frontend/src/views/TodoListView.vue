<script setup lang="ts">
import TodoItem from "@/components/TodoItem.vue"
</script>

<template>
    <input type="text" placeholder="Search Items...">
    <h2>Add Items</h2>
    <form>
        <input type="text">
        <input type="submit" value="Add Item">
    </form>
    <h2>Items</h2>
            <TodoItem todo="hej"></TodoItem>
</template>

<style>
</style>